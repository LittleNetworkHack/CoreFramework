using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Core.Collections;

using Core.Reflection;

namespace Core.Data
{
	public static class CoreDataReader
	{
		#region Primitives

		public static object ReadPrimitive(this SqlDataReader reader, int index, Type primitiveType)
		{
			object value;
			reader.ReadPrimitive(index, primitiveType, out value);
			return value;
		}

		public static object ReadPrimitive(this SqlDataReader reader, string column, Type primitiveType)
		{
			object value;
			reader.ReadPrimitive(column, primitiveType, out value);
			return value;
		}

		public static bool ReadPrimitive(this SqlDataReader reader, int index, Type primitiveType, out object value)
		{
			if (!reader.Read())
			{
				value = ObjectActivator.GetDefaultOrNull(primitiveType);
				return false;
			}

			value = CoreConverter.ConvertTo(reader[index], primitiveType);
			return true;
		}

		public static bool ReadPrimitive(this SqlDataReader reader, string column, Type primitiveType, out object value)
		{
			if (!reader.Read())
			{
				value = ObjectActivator.GetDefaultOrNull(primitiveType);
				return false;
			}

			value = CoreConverter.ConvertTo(reader[column], primitiveType);
			return true;
		}

		#endregion Primitives

		#region Object

		public static object ReadObject(this SqlDataReader reader, Type objectType)
		{
			object instance;
			reader.ReadObject(objectType, out instance);
			return instance;
		}

		public static bool ReadObject(this SqlDataReader reader, Type objectType, out object instance)
		{
			IPropertyKey[] keys = reader.GetKeys(objectType);
			IObjectActivator activator = ObjectActivator.GetActivator(objectType);
			return reader.ReadObject(keys, activator, out instance);
		}

		public static bool ReadObject(this SqlDataReader reader, IPropertyKey[] keys, IObjectActivator activator, out object instance)
		{
			instance = activator.InvokeConstructor();
			if (reader.ReadObject(keys, instance))
				return true;

			// default for structs, consider not using structs
			instance = activator.DefaultValue;
			return false;
		}

		public static bool ReadObject(this SqlDataReader reader, IPropertyKey[] keys, object instance)
		{
			if (!reader.Read())
				return false;

			for (int i = 0; i < reader.FieldCount; i++)
			{
				IPropertyKey key = keys[i];
				if (key == null || key.IsReadOnly)
					continue;

				object value = reader[i];
				value = CoreConverter.ConvertTo(value, key.PropertyType);
				key.SetBoxedValue(instance, value);
			}

			return true;
		}

		#endregion Object

		#region Collection

		public static ICoreCollection ReadCollection(this SqlDataReader reader, Type type)
		{
			if (!typeof(ICoreCollection).IsAssignableFrom(type))
				throw new ArgumentException("Type must be 'ICoreCollection<T>'");

			ICoreCollection collection = ObjectActivator.CreateDirty<ICoreCollection>(type);
			Type itemType = collection.ItemType;
			reader.ReadCollection(collection, itemType);
			return collection;
		}

		public static void ReadCollection(this SqlDataReader reader, IList collection, Type itemType)
		{
			if (CoreConverter.IsPrimitive(itemType))
			{
				while (reader.ReadPrimitive(0, itemType, out object item))
					collection.Add(item);
			}
			else
			{
				IPropertyKey[] keys = reader.GetKeys(itemType);
				IObjectActivator activator = ObjectActivator.GetActivator(itemType);
				while (reader.ReadObject(keys, activator, out object item))
					collection.Add(item);
			}
		}

		#endregion Collection

		#region Data Set/Table

		public static DataSet ReadDataSet(this SqlDataReader reader)
		{
			int cnt = 0;
			DataSet set = new DataSet();

			do
			{
				DataTable table = reader.ReadDataTable();
				if (table == null)
					continue;

				table.TableName = "Table" + cnt++;
				set.Tables.Add(table);
			}
			while (reader.NextResult());

			return set;
		}

		public static DataTable ReadDataTable(this SqlDataReader reader)
		{
			if (!reader.Read())
				return null;

			DataTable table = new DataTable();
			int cnt = reader.FieldCount;
			for (int i = 0; i < cnt; i++)
			{
				string name = reader.GetName(i);
				Type type = reader.GetFieldType(i);
				table.Columns.Add(name, type);
			}

			do
			{
				DataRow row = table.NewRow();
				reader.GetValues(row.ItemArray);
			}
			while (reader.Read());

			return table;
		}

		#endregion Data Set/Table

		#region Shared

		public static object ReadAuto(this SqlDataReader reader, Type type)
		{
			object instance;
			if (CoreConverter.IsPrimitive(type))
				reader.ReadPrimitive(0, type, out instance);
			else if (typeof(ICoreCollection).IsAssignableFrom(type))
				instance = reader.ReadCollection(type);
			else if (type == typeof(DataTable))
				instance = reader.ReadDataTable();
			else if (type == typeof(DataSet))
				instance = reader.ReadDataSet();
			else
				reader.ReadObject(type, out instance);

			return instance;
		}

		public static IPropertyKey[] GetKeys(this SqlDataReader reader, Type type)
		{
			IPropertyKey[] keys = new IPropertyKey[reader.FieldCount];
			PropertyKeyCollection properties = PropertyKey.GetPropertyKeys(type);
			for (int i = 0; i < reader.FieldCount; i++)
			{
				string name = reader.GetName(i);
				IPropertyKey key = properties.FirstOrDefault(K => StringComparer.OrdinalIgnoreCase.Compare(K.Name, name) == 0);
				if (key == null)
					continue;

				keys[i] = key;
			}
			return keys;
		}

		#endregion Shared

		#region Execute

		/// <summary>
		/// Converts result into object using reader[object.propertyName] case insensitive
		/// </summary>
		/// <typeparam name="T">Object used for mapping results</typeparam>
		/// <param name="provider">SqlConnection source</param>
		/// <param name="commandText">SqlCommand text</param>
		/// <param name="type">Text or StoredProcedure</param>
		/// <param name="parameters">SqlCommand parameters</param>
		/// <returns></returns>
		public static CoreCollection<T> ExecuteCommand<T>(this ISqlConnectionProvider provider, string commandText, CommandType type = CommandType.Text, params SqlParameter[] parameters)
		{
			using (SqlConnection connection = provider.CreateConnection(true))
			using (SqlCommand command = connection.CreateCommand(commandText, type, parameters))
			using (SqlDataReader reader = command.ExecuteReader())
			{
				CoreCollection<T> collection = new CoreCollection<T>();
				Type itemType = collection.ItemType;
				if (CoreConverter.IsPrimitive(itemType))
				{
					while (reader.ReadPrimitive(0, itemType, out object item))
						collection.Add((T)item);
				}
				else
				{
					IPropertyKey[] keys = reader.GetKeys(itemType);
					IObjectActivator activator = ObjectActivator.GetActivator(itemType);
					while (reader.ReadObject(keys, activator, out object item))
						collection.Add((T)item);
				}
				return collection;
			}
		}

		/// <summary>
		/// Returns collection of values from reader[index] 
		/// </summary>
		/// <typeparam name="T">Object used for mapping results</typeparam>
		/// <param name="provider">SqlConnection source</param>
		/// <param name="commandText">SqlCommand text</param>
		/// <param name="index">SqlReader column index</param>
		/// <param name="type">Text or StoredProcedure</param>
		/// <param name="parameters">SqlCommand parameters</param>
		/// <returns></returns>
		public static CoreCollection<T> ExecuteCommand<T>(this ISqlConnectionProvider provider, string commandText, int index, CommandType type = CommandType.Text, params SqlParameter[] parameters)
		{
			using (SqlConnection connection = provider.CreateConnection(true))
			using (SqlCommand command = connection.CreateCommand(commandText, type, parameters))
			using (SqlDataReader reader = command.ExecuteReader())
			{
				CoreCollection<T> collection = new CoreCollection<T>();
				while (reader.ReadPrimitive(index, collection.ItemType, out object item))
					collection.Add((T)item);
				return collection;
			}
		}

		/// <summary>
		/// Returns collection of values from reader[name] case insensitive
		/// </summary>
		/// <typeparam name="T">Object used for mapping results</typeparam>
		/// <param name="provider">SqlConnection source</param>
		/// <param name="commandText">SqlCommand text</param>
		/// <param name="index">SqlReader column name</param>
		/// <param name="type">Text or StoredProcedure</param>
		/// <param name="parameters">SqlCommand parameters</param>
		/// <returns></returns>
		public static CoreCollection<T> ExecuteCommand<T>(this ISqlConnectionProvider provider, string commandText, string column, CommandType type = CommandType.Text, params SqlParameter[] parameters)
		{
			using (SqlConnection connection = provider.CreateConnection(true))
			using (SqlCommand command = connection.CreateCommand(commandText, type, parameters))
			using (SqlDataReader reader = command.ExecuteReader())
			{
				CoreCollection<T> collection = new CoreCollection<T>();
				while (reader.ReadPrimitive(column, collection.ItemType, out object item))
					collection.Add((T)item);
				return collection;
			}
		}

		#endregion Execute
	}
}
