using System.IO;
using System.Text;
using NPOI.SS.UserModel;

namespace SznFramework.ExcelConverter
{
    public static class DataTemplateCreator
    {
        public static void Create(string InPath, string InNameSpace, string InName,
            ICell[] InAttrTypes, ICell[] InAttrNames, ICell[] InAttrComment = null)
        {
            if (null == InAttrTypes)
            {
                Log.Error("[DataTemplateCreator]    Attribute type can not be null.");
                return;
            }

            if (null == InAttrNames)
            {
                Log.Error("[DataTemplateCreator]    Attribute name can not be null.");
                return;
            }

            int attrLen = InAttrTypes.Length;
            if (attrLen != InAttrNames.Length)
            {
                Log.Error("[DataTemplateCreator]    The number of attribute types and names must be the same.");
                return;
            }

            if (InAttrComment != null && attrLen != InAttrComment.Length)
            {
                Log.Error("[DataTemplateCreator]    Attribute comments can be null or the number of comments and names must be the same.");
                return;
            }

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("namespace ").Append(InNameSpace).Append("\n")
                .Append("{\n")
                .Append("    public class ").Append(InName).Append("\n")
                .Append("    {\n");

            for (int i = 0; i < attrLen; i++)
            {
                sb.Append("        public ")
                    .Append(InAttrTypes[i].StringCellValue).Append(" ")
                    .Append(InAttrNames[i].StringCellValue).Append(";");
                if (InAttrComment != null && InAttrComment[i] != null)
                    sb.Append("     //").Append(InAttrComment[i].StringCellValue).Append("\n");
                else sb.Append("\n");
            }

            sb.Append("    }\n")
             .Append("}\n");
            
            File.WriteAllText(InPath + InName + ".cs", sb.ToString());
        }
    }
}

