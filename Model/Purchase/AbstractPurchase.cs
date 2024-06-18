namespace s28201_Project.Model;

public abstract class AbstractPurchase
{
    public decimal Price { get; set; }
    private List<Installment> _installments = new ();

    public void AddInstallment(Installment installment)
    {
        installment.Confirm();
        _installments.Add(installment);
    }

    public decimal GetTotalPaidPln()
    {
        return _installments.Sum(installment => installment.Price);
    }

    public abstract decimal GetTotalPredictedPaidPln();
}