namespace Fanzoo.Kernel.Services
{
    public interface IPaymentService<TPaymentRequest, TPaymentResult, TCreateCustomerRequest, TCreateCustomerResult, TCancelPaymentRequest, TCancelPaymentResult> : IService
    {
        ValueTask<TPaymentResult> CreatePaymentAsync(TPaymentRequest request);

        ValueTask<TCreateCustomerResult> CreateCustomerAsync(TCreateCustomerRequest request);

        ValueTask<TCancelPaymentResult> CancelPaymentAsync(TCancelPaymentRequest request);
    }
}
