using Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests
{
    internal class TextMaterialEqualityComparer: IEqualityComparer<TextMaterial>
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
                x.AuthorId == y.AuthorId &&
                x.TextMaterialCategoryId == y.TextMaterialCategoryId &&
                x.Content == y.Content &&
                x.ApprovalStatus == y.ApprovalStatus;
        }

        public int GetHashCode([DisallowNull] TextMaterial obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class TextMaterialCategoryEqualityComparer : IEqualityComparer<TextMaterialCategory>
    {
        public bool Equals(TextMaterialCategory? x, TextMaterialCategory? y)
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
                x.Title == y.Title;
        }

        public int GetHashCode([DisallowNull] TextMaterialCategory obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class UserRepositoryEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User? x, User? y)
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
                x.UserName == y.UserName &&
                x.Email == y.Email;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class CommentRepositoryEqualityComparer : IEqualityComparer<Comment>
    {
        public bool Equals(Comment? x, Comment? y)
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
                x.Content == y.Content &&
                x.UserId == y.UserId &&
                x.TextMaterialId == y.TextMaterialId;
        }

        public int GetHashCode([DisallowNull] Comment obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BanEqualityComparer : IEqualityComparer<Ban>
    {
        public bool Equals(Ban? x, Ban? y)
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
                x.Reason == y.Reason &&
                x.UserId == y.UserId &&
                x.Expires == y.Expires;
        }

        public int GetHashCode([DisallowNull] Ban obj)
        {
            return obj.GetHashCode();
        }
    }
}
