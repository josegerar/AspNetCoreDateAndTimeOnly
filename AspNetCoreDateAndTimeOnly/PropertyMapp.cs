using System;

namespace AspNetCoreDateAndTimeOnly;

public class PropertyMapp
{
    public string Name { get; set; } = "";
    public Type Type { get; set; }

    public bool IsSame(PropertyMapp mapp)
    {
        if (mapp == null)
        {
            return false;
        }
        bool same = mapp.Name == Name && mapp.Type == Type;
        return same;
    }
}