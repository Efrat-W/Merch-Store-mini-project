//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace BlImplementation;

//internal class Tools
//{
//    public static string ToStringProperty<T>(T t)
//    {
//        string str = "";
//        foreach (PropertyInfo item in t.GetType().GetProperties())
//            str += "\n" + item.Name
//            + ": " + item.GetValue(t, null);
//        return str;
//    }


//    public static DO.Product ProductBoToDo(this BO.Product prod)
//    {
//        return new DO.Product()
//        {
//            ID = prod.ID,
//            Name = prod.Name,
//            Category = (DO.category)prod.Category,
//            Price = prod.Price
//        };
//    }

//    public static BO.Product ProductDoToBo(this DO.Product prod)
//    {
//        return new BO.Product()
//        {
//            ID = prod.ID,
//            Name = prod.Name,
//            Category = (BO.category)prod.Category,
//            Price = prod.Price
//        };
//    }
//}
