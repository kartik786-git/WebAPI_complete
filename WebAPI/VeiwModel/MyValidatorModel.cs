namespace WebAPI.VeiwModel
{
    public class MyValidatorModel
    {
        public string? Name { get; set; }
        public int? Age { get; set; }

        public string? Email { get; set; }

        public OneNestedValidatorModel? OneNestedValidatorModel { get; set; } = null!;

        public SecondNestedValidatorModel? SecondNestedValidatorModel { get; set; }
    }
}
