using System;

namespace RovingSpecialCustomers.Models;

[Serializable]
public sealed class RovingVisitState
{
    public int VisitSequence;
    public int ScheduledFromDay = -1;
    public int NoticeDay = -1;
    public int ArrivalDay = -1;
    public string CrewId = string.Empty;
    public string[] PreferredEffectIds = Array.Empty<string>();
    public float BudgetMultiplier = 1f;
    public float VisitBudget;
    public float RemainingBudget;
    public bool NoticeSent;
    public bool Active;
    public bool DealOffered;
    public bool DealCompleted;
    public bool ExclusivePurchased;
    public string OfferedProductId = string.Empty;
    public int OfferedQuantity;
    public float OfferedPayment;

    public bool HasScheduledVisit => ArrivalDay >= 0 && !string.IsNullOrWhiteSpace(CrewId);

    public void ResetCurrentVisit()
    {
        ScheduledFromDay = -1;
        NoticeDay = -1;
        ArrivalDay = -1;
        CrewId = string.Empty;
        PreferredEffectIds = Array.Empty<string>();
        BudgetMultiplier = 1f;
        VisitBudget = 0f;
        RemainingBudget = 0f;
        NoticeSent = false;
        Active = false;
        DealOffered = false;
        DealCompleted = false;
        ExclusivePurchased = false;
        OfferedProductId = string.Empty;
        OfferedQuantity = 0;
        OfferedPayment = 0f;
    }
}
