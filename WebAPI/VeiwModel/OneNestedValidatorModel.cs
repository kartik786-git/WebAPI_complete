namespace WebAPI.VeiwModel
{
    public class OneNestedValidatorModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Email { get; set; }

        public InsideOneModel AnotherModel { get; set; }
    }
}
