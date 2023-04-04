namespace Data.DTO
{
    public class XmlData
    {
        public class Response
        {
            public Row row { get; set; }
        }

        public class XmlFile
        {
            public Response response { get; set; }
        }
        public class Row
        {
            public List<XmlDto> row { get; set; }
        }
    }
}
