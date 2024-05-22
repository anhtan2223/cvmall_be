namespace Framework.Core.Helpers
{
    public static class CsvHelpers
    {
        public static List<T> Import<T>(string[]? lines)
        {
            List<T> list = new List<T>();
            var headerLine = lines[0];
            var columns = headerLine.Split(',').ToList().Select((v, i) => new { Position = i, Name = v });
            var dataLines = lines.Skip(1).ToList();
            var type = typeof(T);
            var props = type.GetProperties();
            dataLines.ForEach(line =>
            {
                T obj = (T)Activator.CreateInstance(type);
                var data = line.Split(',').ToList();
                var i = 0;
                foreach (var prop in props)
                {
                    var column = columns.SingleOrDefault(c => c.Position.Equals(i));
                    var value = data[column.Position];
                    var typeOfProp = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(value, typeOfProp));
                    i++;
                }
                list.Add(obj);
            });
            return list;
        }

        public static async Task<byte[]> ExportCsv<T>(List<T> listData, List<CsvItem> headers)
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    // Header
                    var header = string.Join(",", headers.Select(x => (x.header)));
                    sw.WriteLine(header);

                    // Data
                    var listCode = headers.Select(x => x.key).ToList();
                    foreach (var dataItem in listData)
                    {
                        Type entType = dataItem.GetType();
                        var props = entType.GetProperties().Where(p => listCode.Any(p2 => p2 == p.Name)).ToList();
                        string str = "";
                        for (int j = 0; j < props.Count; j++)
                        {
                            var propItem = props[j];
                            var codeIndex = listCode.FindIndex(x => x.Equals(propItem.Name));
                            if (codeIndex > j)
                            {
                                propItem = props.Where(x => x.Name.Equals(listCode[j])).FirstOrDefault();
                                props[codeIndex] = propItem;
                            }
                            var propValue = propItem.GetValue(dataItem);
                            str += propValue != null ? propValue : "";
                            str += ",";
                        }
                        sw.WriteLine(str);
                    }
                    await sw.FlushAsync();
                }
                await ms.FlushAsync();
                return ms.ToArray();
            }
        }
    }
}
