using Gateways;

namespace Domain;

internal sealed class GetByBarcodeUseCase(SpoolmanClient spoolmanClient) : IUseCase<GetByBarcodeInput>
{
    public async Task<IOutput> ExecuteAsync(GetByBarcodeInput input)
    {
        var spools = await spoolmanClient.GetByBarcodeAsync(input.Barcode);

        return new GetAllSpoolsOutput(spools);
    }
}