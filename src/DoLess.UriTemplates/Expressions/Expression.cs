using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DoLess.UriTemplates.Entities;

namespace DoLess.UriTemplates.Expressions
{
    internal abstract class Expression : IExpression
    {
        protected readonly StringBuilder builder;
        protected readonly IReadOnlyDictionary<string, object> variables;
        protected readonly string first;
        protected readonly char sep;
        protected readonly bool named;
        protected readonly string ifemp;
        protected readonly bool allowReserved;

        public Expression(IReadOnlyDictionary<string, object> variables, string first, char sep, bool named, string ifemp, bool allowReserved)
        {
            this.builder = new StringBuilder();
            this.variables = variables;
            this.first = first;
            this.sep = sep;
            this.named = named;
            this.ifemp = ifemp;
            this.allowReserved = allowReserved;
        }

        public void Expand(VarSpec varSpec)
        {
            if (this.variables.TryGetValue(varSpec.Name, out object value) && value != null)
            {
                if (this.builder.Length == 0)
                {
                    this.builder.Append(this.first);
                }
                else
                {
                    this.builder.Append(this.sep);
                }

                this.Expand(varSpec, value);
            }
        }

        public string Process()
        {
            var result = this.builder.ToString();
            this.builder.Clear();
            return result;
        }

        private void Expand(VarSpec varSpec, string value)
        {
            this.ExpandName(varSpec, value == string.Empty);
            this.ExpandStringValue(value, varSpec.MaxLength);
        }

        private void Expand(VarSpec varSpec, IEnumerable values)
        {
            bool isEmpty = !values.Any();
            this.Expand<IEnumerable, object>(varSpec, isEmpty, values, IEnumerableExtensions.ForEachIEnumerable, x => varSpec.Name, x => x.ToString());
        }

        private void Expand(VarSpec varSpec, IDictionary<string, string> values)
        {
            bool isEmpty = values.Count == 0;
            this.Expand<IDictionary<string, string>, KeyValuePair<string, string>>(varSpec, isEmpty, values, IEnumerableExtensions.ForEach, x => x.Key, x => x.Value);
        }

        private void Expand(VarSpec varSpec, IReadOnlyDictionary<string, string> values)
        {
            bool isEmpty = values.Count == 0;
            this.Expand<IReadOnlyDictionary<string, string>, KeyValuePair<string, string>>(varSpec, isEmpty, values, IEnumerableExtensions.ForEach, x => x.Key, x => x.Value);
        }

        private void Expand<TValues, T>(VarSpec varSpec, bool isEmpty, TValues values, Action<TValues, Action<T>> iterator, Func<T, string> getKey, Func<T, string> getValue)
        {
            bool isNameExploded = this.named && varSpec.IsExploded;
            char separator = varSpec.IsExploded ? this.sep : ',';

            if (!varSpec.IsExploded)
            {
                this.ExpandName(varSpec, isEmpty);
            }

            iterator(values, x =>
            {
                if (isNameExploded)
                {
                    this.builder.AppendEncoded(getKey(x), this.allowReserved);
                    this.builder.Append('=');
                }

                this.builder.AppendEncoded(getValue(x), this.allowReserved);
                this.builder.Append(separator);
            });

            if (!isEmpty)
            {
                this.builder.RemoveLastChar();
            }
        }

        private void Expand(VarSpec varSpec, object value)
        {
            switch (value)
            {
                case string val:
                    this.Expand(varSpec, val);
                    break;

                case IDictionary<string, string> val:
                    this.Expand(varSpec, val);
                    break;

                case IReadOnlyDictionary<string, string> val:
                    this.Expand(varSpec, val);
                    break;

                case IEnumerable val:
                    this.Expand(varSpec, val);
                    break;

                default:
                    // If all above fails, convert the object to its string representation.
                    this.Expand(varSpec, value.ToString());
                    break;
            }
        }

        private void ExpandName(VarSpec varSpec, bool isValueEmpty)
        {
            if (this.named)
            {
                this.builder.Append(varSpec.Name);

                if (isValueEmpty)
                {
                    this.builder.Append(this.ifemp);
                }
                else
                {
                    this.builder.Append('=');
                }
            }
        }

        private void ExpandStringValue(string value, int maxLength = 0)
        {
            if (maxLength > 0 && maxLength < value.Length)
            {
                value = value.Substring(0, maxLength);
            }

            this.builder.AppendEncoded(value, this.allowReserved);
        }
    }
}
