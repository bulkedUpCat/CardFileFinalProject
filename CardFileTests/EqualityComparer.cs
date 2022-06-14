using Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests
{
    internal class TextMaterialEqualityComparer : IEqualityComparer<TextMaterial>
    {
        public bool Equals(TextMaterial? x, TextMaterial? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id &&
                x.Title == y.Title &&
                //x.AuthorId == y.AuthorId &&
                x.TextMaterialCategoryId == y.TextMaterialCategoryId &&
                x.Content == y.Content;
        }

        public int GetHashCode([DisallowNull] TextMaterial obj)
        {
            return obj.GetHashCode();
        }
    }
}
