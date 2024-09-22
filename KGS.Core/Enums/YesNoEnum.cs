using System.ComponentModel;

namespace KGS.Core
{
    public enum YesNoEnum
    {
        Yes = 1,
        No = 0
    }

    public enum EffectiveRatingEnum
    {
        [Description("very effective")]
        VeryEffective = 1,
        [Description("effective")]
        Effective = 2,
        [Description("only somewhat effective")]
        OnlySomewhatEffective = 3,
    }

    public enum MarijuanaMethodTypeEnum
    {
        [Description("smoke")]
        smoke = 1,
        [Description("vaporizer")]
        vaporizer = 2,
        [Description("ingested")]
        ingested = 3,
        [Description("tincture")]
        tincture = 4,
        [Description("topical")]
        topical = 5,
    }

    public enum GenderName
    {
        [Description("Male")]
        male = 1,
        [Description("Female")]
        female = 2
    }
    public enum PublishEnum
    {
        [Description("Yes")]
        yes = 1,
        [Description("No")]
        no = 0
    }

    public enum PlanTypes
    {
        [Description("NotSubscribed")]
        NotSubscribed = 0,
        [Description("Monthly")]
        Monthly = 1,
        [Description("Yearly")]
        Yearly = 2,
        [Description("Free")]
        Free = 3,
        [Description("Free-Trial")]
        Trial = 4
    }
  
}