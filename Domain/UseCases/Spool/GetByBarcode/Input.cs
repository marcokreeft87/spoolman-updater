namespace Domain;

public class GetByBarcodeInput : IInput
{
    public GetByBarcodeInput(string barcode)
    {
        Barcode = barcode;
    }
    public string Barcode { get; }
}