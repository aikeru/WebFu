﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace WebFormsUtilities.Json
{
    public class JSONObjectEmpty : JSONObjectBase
    {
        public JSONObjectEmpty()
        {
        }
        public override string Render()
        {
            return "{}";
        }
    }
    public class JSONObject : JSONObjectBase
    {
        public JSONObject()
        {
        }
        public JSONObject(object jsonProperties)
        {
            foreach (PropertyInfo pi in jsonProperties.GetType().GetProperties())
            {
                object o = pi.GetValue(jsonProperties, null);
                if (o != null)
                {
                    if (o.GetType().IsSubclassOf(typeof(JSONObjectBase)))
                    {
                        JSONProperties.Add(pi.Name, (JSONObjectBase)o);
                    }
                    else
                    {
                        JSONProperties.Add(pi.Name, new JSONObject() { Value = o });
                    }
                }
                else
                {
                    JSONProperties.Add(pi.Name, new JSONObject() { Value = null });
                }
            }
        }
        public void Attr(string name, JSONObject value)
        {
            if (JSONProperties.Keys.Contains(name))
            {
                JSONProperties[name] = value;
            }
            else
            {
                JSONProperties.Add(name, value);
            }
        }
        public void Attr(string name, object value)
        {
            if (JSONProperties.Keys.Contains(name))
            {
                JSONProperties[name] = new JSONObject() { Value = value };
            }
            else
            {
                JSONProperties.Add(name, new JSONObject() { Value = value });
            }
        }
    }
    public class JSONObjectBase
    {
        private Dictionary<string, JSONObjectBase> _JSONProperties = new Dictionary<string, JSONObjectBase>();

        public Dictionary<string, JSONObjectBase> JSONProperties
        {
            get { return _JSONProperties; }
            set { _JSONProperties = value; }
        }

        /// <summary>
        /// Will not be used if JSONProperties.Count > 0
        /// </summary>
        public object Value { get; set; }

        public JSONObjectBase Attr(string name)
        {
            return JSONProperties[name];
        }

        public override string ToString()
        {
            return Render();
        }

        public virtual string RenderJSONValue(object val)
        {
            StringBuilder sb = new StringBuilder();
            if (val == null) { return "null"; }
            if (val.GetType() == typeof(bool))
            {
                if (Convert.ToBoolean(val)) { return "true"; }
                return "false";
            }
            Type[] numberArrays = new Type[] {
                    typeof(int[]), typeof(long[]),typeof(double[]),typeof(float[]),typeof(byte[]),typeof(short[]),typeof(decimal[])
                };
            Type[] numbers = new Type[] {
                   typeof(int), typeof(long), typeof(double), typeof(float), typeof(byte),typeof(short),typeof(decimal)
                };
            if (numberArrays.Contains(val.GetType()))
            {
                sb.Append("[");
                bool firstNum = true;
                foreach (object x in (IEnumerable)val)
                {
                    if (firstNum) { sb.Append(x.ToString()); }
                    if (!firstNum) { sb.Append("," + x.ToString()); }
                    firstNum = false;

                }
                sb.Append("]");
            }
            else if (val.GetType() == typeof(string))
            {
                return "\"" + val.ToString() + "\"";
            }
            else if (val as IEnumerable != null)
            {
                sb.Append("[");
                bool firstArr = true;
                foreach (object x in (IEnumerable)val)
                {
                    if (firstArr) { sb.Append(RenderJSONValue(x)); }
                    if (!firstArr) { sb.Append("," + RenderJSONValue(x)); }
                    firstArr = false;
                }
                sb.Append("]");
            }
            else if (val.GetType().IsSubclassOf(typeof(JSONObjectBase)))
            {
                sb.Append(((JSONObjectBase)val).Render());
            }
            else if (numbers.Contains(val.GetType()))
            {
                sb.Append(val.ToString());
            }
            else
            {
                sb.Append("\"" + val.ToString() + "\"");
            }
            return sb.ToString();
        }

        public virtual string Render()
        {
            StringBuilder sb = new StringBuilder();
            bool firstProp = true;
            if (JSONProperties.Count > 0)
            {
                sb.Append("{");
                foreach (KeyValuePair<string, JSONObjectBase> kvp in JSONProperties)
                {
                    if (firstProp)
                    {
                        sb.Append("\"" + kvp.Key + "\":");
                        firstProp = false;
                    }
                    else
                    {
                        sb.Append(",\"" + kvp.Key + "\":");
                    }
                    sb.Append(kvp.Value.Render());
                }
                sb.Append("}");
            }
            else
            {
                return RenderJSONValue(Value);
            }

            if (sb.ToString().Contains("\r") || sb.ToString().Contains("\n"))
            { throw new Exception("JSON cannot contain return characters \\r \\n."); }

            return sb.ToString();
        }
    }
}