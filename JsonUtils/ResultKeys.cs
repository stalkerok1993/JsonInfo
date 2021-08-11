using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonUtils
{
    public sealed class ResultKeys
    {
        public const string AccountingFrameworkName = "AccountingFramework";
        public const string AnalysisDateName = "AnalysisDate";
        public const string ExternalIdName = "ExternalId";
        public const string PricingProfileName = "PricingProfile";
        public const string SimulationLabelName = "SimulationLabel";
        public string AccountingFramework { get; }
        public DateTime AnalysisDate { get; }
        public string ExternalId { get; }
        public string PricingProfile { get; }
        public string SimulationLabel { get; }
        public int Position { get; }
        public string File { get; }

        public ResultKeys(string accountingFramework, DateTime analysisDate, string externalId, string pricingProfile, string simulationLabel, int position, string file)
        {
            AccountingFramework = accountingFramework;
            AnalysisDate = analysisDate;
            ExternalId = externalId;
            PricingProfile = pricingProfile;
            SimulationLabel = simulationLabel;
            Position = position;
            File = file;
        }

        public override bool Equals(object obj)
        {
            ResultKeys compared = obj as ResultKeys;
            if (compared == null) return false;
            return AccountingFramework == compared.AccountingFramework &&
                AnalysisDate == compared.AnalysisDate &&
                ExternalId == compared.ExternalId &&
                PricingProfile == compared.PricingProfile &&
                SimulationLabel == compared.SimulationLabel;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountingFramework.GetHashCode(),
                AnalysisDate.GetHashCode(),
                ExternalId.GetHashCode(),
                PricingProfile.GetHashCode(),
                SimulationLabel.GetHashCode());
        }

        public override string ToString()
        {
            return $"{File}[{Position}]: \"{AccountingFrameworkName}\": \"{AccountingFramework}\", \"{AnalysisDateName}\": \"{AnalysisDate}\", \"{ExternalIdName}\": \"{ExternalId}\", \"{PricingProfileName}\": \"{PricingProfile}\", \"{SimulationLabelName}\": \"{SimulationLabel}\"";
        }
    }
}
