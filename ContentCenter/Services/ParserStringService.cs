using System.Collections.Generic;

namespace ContentCenter.Services
{
    public class ParserStringService: IParserStringService
    {
        public List<object> ParseString(string Str)
        {
            var Arr = Str.Split(',');

            int tempVar = 0;
            var List = ParseStringInternal(Arr, ref tempVar, Arr.Length - 1);

            return List;
        }

        private List<object> ParseStringInternal(string[] Arr, ref int Position, int ArrLength)
        {
            //TODO - не обрабатываются ситуации с двойными кавычками и переносами строк в тексте 
            var List = new List<object>();

            while (true)
            {

                var Val = Arr[Position].Trim();
                if (Val.StartsWith("{"))
                {

                    Arr[Position] = Val.Substring(1);

                    List.Add(ParseStringInternal(Arr, ref Position, ArrLength));

                }
                else if (string.IsNullOrEmpty(Val))
                {
                    Position = Position + 1;

                }
                else
                {

                    var Pos = Val.IndexOf("}");
                    if (Pos > -1)
                    {

                        var Vl2 = Val.Substring(0, Pos);
                        if (!string.IsNullOrEmpty(Vl2))
                        {
                            List.Add(Vl2);
                        }

                        Arr[Position] = Val.Substring(Pos + 1);
                        break;
                    }
                    else
                    {
                        List.Add(Val);
                        Position = Position + 1;
                    }
                }

                if (Position >= ArrLength)
                {
                    break;
                }
            }

            return List;
        }
    }
}
