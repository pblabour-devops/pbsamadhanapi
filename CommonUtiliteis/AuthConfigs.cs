namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class AuthConfigs
    {
        public const string LabourWingRoles = "ADLW,LBIN,ALLC,DLHS,SPTU,SOTU,DHLP,DLTU,APTU,DRFT,DLHL,ACFA,ADLC,DLFI,SUPT";
        public const string FactoryWingRoles = "DDRF,ADRF,DLHF,SUPTGEN,DHF_RPT,DLHA,MPRM,PMGR,MPRC";
        public const string HQRoles = "LBCR,PSLD,ADDF,DLBP,JDRF_PSIEC,DHF_BP_HQ,JDRF,DL_PSIEC,DPLC,ARHQ";
        public const string OtherDeptRoles = "ATP,CGM,DTP,JDM,SDO,ATP_PSIEC,EO";
        public const string LWBRoles = "WBDH";
        public const string AllLabourOfficialRoles = LabourWingRoles + "," + FactoryWingRoles + "," + HQRoles + "," + OtherDeptRoles + "," + LWBRoles;
    }
}
