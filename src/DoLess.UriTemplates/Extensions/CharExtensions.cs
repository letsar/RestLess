namespace DoLess.UriTemplates
{
    internal static class CharExtensions
    {
        public static bool IsDigit(this char self)
        {
            // Note that char.IsDigit can return true for the Thai digit ๑.
            // It is not what we want here.
            return (self >= '0' && self <= '9');
        }

        public static bool IsAlpha(this char self)
        {
            return (self >= 'a' && self <= 'z') ||
                   (self >= 'A' && self <= 'Z');
        }

        public static bool IsAlphaOrDigit(this char self)
        {
            return self.IsAlpha() || self.IsDigit();
        }

        public static bool IsHexDigit(this char self)
        {
            return (self >= 'a' && self <= 'f') ||
                   (self >= 'A' && self <= 'F') ||
                   self.IsDigit();
        }

        public static bool IsUnreserved(this char self)
        {
            switch (self)
            {
                case '-':
                case '.':
                case '_':
                case '~':
                    return true;

                default:
                    return self.IsAlphaOrDigit();
            }
        }

        public static bool IsGenDelim(this char self)
        {
            switch (self)
            {
                case ':':
                case '/':
                case '?':
                case '#':
                case '[':
                case ']':
                case '@':
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsSubDelim(this char self)
        {
            switch (self)
            {
                case '!':
                case '$':
                case '&':
                case '\'':
                case '(':
                case ')':
                case '*':
                case '+':
                case ',':
                case ';':
                case '=':
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsReserved(this char self)
        {
            return self.IsGenDelim() || self.IsSubDelim();
        }

        public static bool IsOpNotSupported(this char self)
        {
            return self.IsOpReserved() ||
                   self.IsOpUnreserved();
        }

        public static bool IsOpLevel1(this char self)
        {
            return self == char.MinValue;
        }

        public static bool IsOpLevel2(this char self)
        {
            return self == '+' ||
                   self == '#';
        }

        public static bool IsOpLevel3(this char self)
        {
            return self == '.' ||
                   self == '/' ||
                   self == ';' ||
                   self == '?' ||
                   self == '&';
        }

        public static bool IsOpReserved(this char self)
        {
            return self == '=' ||
                   self == ',' ||
                   self == '!' ||
                   self == '@' ||
                   self == '|';
        }

        public static bool IsOpUnreserved(this char self)
        {
            return self == '$' ||
                   self == '(' ||
                   self == ')';
        }

        public static bool IsValidVarSpecChar(this char self)
        {
            return self.IsAlphaOrDigit() ||
                   self == '_' ||
                   self == '.';
        }
    }
}
