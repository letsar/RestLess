using System;

namespace DoLess.UriTemplates.Entities
{
    internal class VarSpec
    {
        public VarSpec(string name, int maxLength, bool isExploded)
        {
            this.Name = name;
            this.MaxLength = Math.Min(maxLength, name.Length);
            this.IsExploded = isExploded;
        }

        public string Name { get; }

        public int MaxLength { get; }

        public bool IsExploded { get; }
    }
}
