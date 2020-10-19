using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Djora 25.09.19 28.11.19
namespace Bankom.Class
{
    class clsElementiForme
    {
        public string DajVrednostPropertija(Object obj, string ImeProp)
        {
            //Dusan
            long bb = ((frmChield)obj).iddokumenta;

            //Za Field-ove
            FieldInfo fld = typeof(frmChield).GetField("iddokumenta");
           Console.WriteLine(fld.GetValue(obj));

            Type t = obj.GetType();
            Console.WriteLine("Type is: {0}", t.Name);
            PropertyInfo[] props = t.GetProperties();
            Console.WriteLine("Properties (N = {0}):", props.Length);
            foreach (var prop in props)
            {
                //if (prop.GetIndexParameters().Length == 0)
                //    //Console.WriteLine("   {0} ({1}): {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(obj));
                //else
                //{
                //    Console.WriteLine("   {0} ({1}): <Indexed>", prop.Name, prop.PropertyType.Name);
                //    //MessageBox.Show(t.GetProperty("iddokumenta").GetValue(obj).ToString());
                //    //MessageBox.Show(t.GetProperty(ImeProp).GetValue(obj).ToString());
                //}

                //Djora 02.12.19

                Type tt = obj.GetType();
                PropertyInfo[] propin = tt.GetProperties();
                //return tt.GetProperty(ImeProp).GetValue(obj).ToString();

                //Type tp = obj.GetType();
                //FieldInfo[] fil = tp.GetFields();
                //return tp.GetField(".iddokumenta").GetValue(obj).ToString();
            }
            return "aaa";
        }
    }
}
