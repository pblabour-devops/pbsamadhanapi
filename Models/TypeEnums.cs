using pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public enum EstablishmentTypeEnum
    {
        [Description("Factory")]
        FACTORY = 1,

        [Description("Building and other construction")]
        BUILDING_AND_OTHER_CONSTRUCTION = 2,

        [Description("Contract work")]
        CONTRACT_WORK = 3,

        [Description("Mining")]
        MINING = 4,

        [Description("Dock work")]
        DOCK_WORK = 5,

        [Description("Others")]
        OTHERS = 6
    }

    public enum EstablishmentConstitutionTypeEnum
    {
        [Description("Proprietorship")]
        PROPRIETORSHIP = 1,

        [Description("Partnership")]
        PARTNERSHIP = 2,

        [Description("Limited company")]
        LIMITED_COMPANY = 3,

        [Description("Government department")]
        GOVERNMENT_DEPARTMENT = 4,

        [Description("Local authority")]
        LOCAL_AUTHORITY = 5,

        [Description("Cooperative")]
        COOPERATIVE = 6,

        [Description("Private limited company")]
        PRIVATE_LIMITED_COMPANY = 7,
    }

    public enum EstablishmentBuildingTypeEnum
    {
        [Description("Own")]
        OWN = 1,

        [Description("Rented")]
        RENTED = 2,

        [Description("Allotted")]
        ALLOTTED = 3,

        [Description("Through mutual agreement")]
        THROUGH_MUTUAL_AGREEMENT = 4,
    }

    public enum ApplicationTypeEnum
    {
        [Description("No Service")]
        [CustomeAttribute_EnumShortDesc("No Service")]
        DEFAULT = 0,

        [Description("Registration of establishment")]
        [CustomeAttribute_EnumShortDesc("Registration of establishment")]
        REG_ESTB_OSH = 1,

        [Description("Application for contractor")]
        [CustomeAttribute_EnumShortDesc("Application for contractor")]
        CONTRACT_LABOUR_OSH = 2,

        [Description("Registration of building plan")]
        [CustomeAttribute_EnumShortDesc("Registration of building plan")]
        BUILDING_PLAN_OSH = 3,

        [Description("Registration of common licence")]
        [CustomeAttribute_EnumShortDesc("Registration of common licence")]
        COMMON_LICENCE_OSH = 4,

        [Description("Combined Proposed Approval of Building Plans (HUD and Factories)")]
        [CustomeAttribute_EnumShortDesc("Combined Building Plan")]
        BUILDING_PLAN_HUD = 5,

        [Description("Registration of Shop Licence")]
        [CustomeAttribute_EnumShortDesc("Registration of Shop")]
        SHOP_LICENCE = 6,

        [Description("Trade Union")]
        [CustomeAttribute_EnumShortDesc("Trade Union")]
        TRADE_UNION = 8,

        //[Description("Registration of Proncipal Employer")]
        //[CustomeAttribute_EnumShortDesc("Registration of Proncipal Employer")]
        //CONTRACT_LABOUR_PE = 9,


        //[Description("Application for Migrant Workmen")]
        //[CustomeAttribute_EnumShortDesc("Application for Migrant Workmen")]
        //MIGRANT_WORKMEN = 11,


        [Description("Application for BOCW")]
        [CustomeAttribute_EnumShortDesc("Application for BOCW")]
        BOCW_ESTABLISHMENT_ACT = 35,

        [Description("Registration of Factory Licence")]
        [CustomeAttribute_EnumShortDesc("Registration of Factory")]
        FACTORY_LICENCE = 70,

        [Description("Grant of permission for Women to work in Night Shift (Under Shop & commercial establishment act)")]
        [CustomeAttribute_EnumShortDesc("Women Permission for Night Shift (Shop)")]
        WOMEN_NIGHT_SHIFT_SHOP = 61,

        [Description("Grant of permission for Women to work in Night Shift (Under Factory act  - 1948)")]
        [CustomeAttribute_EnumShortDesc("Women Permission for Night Shift (Factory)")]
        WOMEN_NIGHT_SHIFT_FACTORY = 62,

        [Description("Application for Building Plans Under The Factory Act - 1948 (Fresh)")]
        [CustomeAttribute_EnumShortDesc("Building Plans under Factory Act")]
        BUILDING_PLAN = 1000,

        [Description("Application for Contract Labour")]
        [CustomeAttribute_EnumShortDesc("Application for Contract Labour")]
        CONTRACT_LABOUR = 38,

        [Description("Application for Principal Employer")]
        [CustomeAttribute_EnumShortDesc("Application for Principal Employer")]
        PRINCIPAL_EMPLOYER = 37,

        [Description("Approval of Building Plan of Existing Building with Stability Certificate (Addition)")]
        [CustomeAttribute_EnumShortDesc("Approval of Building Plan of Existing Building with Stability Certificate (Addition)")]
        BP_EXISTING_WITH_STABILITY_ADDITION = 1003,

        [Description("Approval of Building Plan of Existing Building with Stability Certificate (Amendment)")]
        [CustomeAttribute_EnumShortDesc("")]
        BP_EXISTING_WITH_STABILITY_AMENDMENT = 1004,

        [Description("Application for Stability Certificate of Building Plans Under The Factory Act - 1948 (Fresh)")]
        [CustomeAttribute_EnumShortDesc("")]
        BP_STABITLITY_CERTIFICATE = 1005,

        [Description("Application for Approval of Proposed Building Plans Under The Factory Act - 1948")]
        [CustomeAttribute_EnumShortDesc("Proposed Building Plans")]
        BUILDING_PLAN_PROPOSED = 71,

        [Description("Application for Approval of Existing Building Plans Under The Factory Act - 1948")]
        [CustomeAttribute_EnumShortDesc("Existing Building Plans")]
        BUILDING_PLAN_EXISTING = 72,

        [Description("Approval of Building Plan of Existing Building with Stability Certificate (Addition/Amendment)")]
        [CustomeAttribute_EnumShortDesc("Addition-Amendment Building Plans")]
        BUILDING_PLAN_ADDITION_AMENDMENT = 73,

        [Description("Application for Approval of Building Plans Under The Factory Act - 1948 (Proposed/Existing/Addition-Amendment)")]
        [CustomeAttribute_EnumShortDesc("Building Plan Plans Under The Factory Act")]
        BUILDING_PLAN_UNDER_FACTORY_ACT = 74,

        [Description("Submission of Stability Certificate Under Factories Act-1948")]
        [CustomeAttribute_EnumShortDesc("")]
        BP_DECLARATION_STABILITY_CERTIFICATE = 76,

        [Description("Factory Inspection")]
        [CustomeAttribute_EnumShortDesc("Factory Inspection")]
        FACTORY_INSPECTION = 85,

        [Description("Application for Principal Employer Under Migrant Workmen")]
        [CustomeAttribute_EnumShortDesc("Application for Principal Employer Under ISM")]
        ISM_PRINCIPAL_EMPLOYER = 39,

        [Description("Application for Contract Labour Under Migrant Workmen")]
        [CustomeAttribute_EnumShortDesc("Application for Contract Labour Under ISM")]
        ISM_CONTRACT_LABOUR = 40,


        [Description("Application for Motor Transport")]
        [CustomeAttribute_EnumShortDesc("Application for Motor Transport")]
        MOTOR_TRANSPORT = 36,

        [Description("Combined Building Plans (PSIEC and Factories)")]
        [CustomeAttribute_EnumShortDesc("Combined Building Plan For PSIEC And Factories")]
        BUILDING_PLAN_PSIEC = 81,

        [Description("Form 1 Registration")]
        [CustomeAttribute_EnumShortDesc("Form 1 Registration")]
        OSH_FORM_1_Registration = 101,

        [Description("Labour Services")]
        [CustomeAttribute_EnumShortDesc("Labour Services")]
        LABOUR_SERVICES = 1001,

        #region For samadhaan

        [Description("Samadhaan Complaints")]
        [CustomeAttribute_EnumShortDesc("Samadhaan Complaints")]
        SAMADHAN_COMPLAINTS = 100001,

        #endregion
    }

    public enum AppActionTypeEnum
    {
        [Description("Application saved as draft")]
        DEFAULT = 0,

        [Description("Application saved as draft")]
        APP_SAVE_DRAFT = 1,

        [Description("Application saved as lock and fees pending")]
        APP_SAVE_LOCK_FEE_PENDING = 2,

        [Description("Application submitted")]
        APP_SUBMITTED = 3,

        [Description("Application submitted (Fee not applicable)")]
        APP_SUBMITTED_FEE_NOT_APPLICABLE = 5,

        [Description("Objection resolved and application resubmitted")]
        OBJECTION_RESOLVED_APPLICATION_RESUBMITTED = 6,

        [Description("Application approved")]
        APP_APPROVED = 200,

        [Description("Objection in Form")]
        APP_OBJECTION = 404,

        [Description("Application rejected")]
        APP_REJECT = 201,

        [Description("Fee raised")]
        FEE_RAISED = 400,

        [Description("Deregistered")]
        DEREGISTERED = 202,

        [Description("File Transferred")]
        FILE_TRANSFERRED = 203,

        [Description("Deemed Initialized")]
        DEEMED_INITIALIZED = 205,

        [Description("Deemed Completed")]
        DEEMED_COMPLETED = 206,

        [Description("Raise objection with balance fees")]
        APP_OBJECTION_WITH_BALANCE_FEE = 402,

        [Description("Objection resolve and balance fee paid")]
        APP_OBJECTION_RESOLVED_BALANCE_FEE_PAID = 403,

        [Description("Raised Fees Paid (BuildinPlan-HUD) ")]
        RAISED_FEE_PAID = 8,

        [Description("Raise Fees Paid Offline (BuildinPlan-HUD)")]
        RAISED_FEE_PAID_OFFLINE = 409,

        [Description("Factory Licence Application submitted (Fee not applicable)")]
        FACTORY_APP_SUBMITTED_FEE_NOT_APPLICABLE = 405,

        [Description("Application submitted And Approval Under Process")]
        APP_SUBMITTED_APPROVAL_UNDER_PROCESS = 11,

        [Description("Application submitted Fee Not Applicable And Approval Under Process")]
        APP_SUBMITTED_FEE_NOT_APPLICABLE_APPROVAL_UNDER_PROCESS = 12,

        [Description("Application Approved By Auto Approval")]
        AUTO_APPROVAL_COMPLETED = 208,

        [Description("Acknowledgment Submitted To Department (No Further Action Required)")]
        ACKNOWLEDGMENR_SUBMITTED_TO_DEPARTMENT_NO_ACTION_REQUIRED = 209,

        [Description("Forward For Approval")]
        FORWARD_FOR_APPROVAL = 101,

        [Description("Forwarded With Objections")]
        FORWARD_FOR_OBJECTION = 102,

        [Description("Application scrutiny completed and forwarded for further action")]
        SCRUTINY_COMPLETED = 106,

        [Description("Forward as Recommendation of Rejection")]
        REJECT_RECOMMENDATION = 207,

        [Description("Marked Back With Query")]
        MARK_BACK = 100,

        [Description("Recommendation for Approval")]
        APPROVE_RECOMMENDATION = 104,

        [Description("Forward to Dealing Hand")]
        FORWARD_TO_DEALING_HAND = 107,

        [Description("Fee Raised And Sent To Applicant")]
        FEE_RAISED_SENT_TO_APPLICANT = 401,

        [Description("Replied To Query")]
        REPLIED_TO_QUERY = 108,

        [Description("Raised Fees Paid (BP)")]
        RAISED_FEE_PAID_BP = 406,

        [Description("Declined due to incomplete application")]
        DECLINED_DUE_TO_INCOMPLETE = 212,

        [Description("Forwarded to field officer")]
        FORWARDED_TO_FIELD_OFFICER = 110,

        [Description("Forwarded to nodal officer")]
        FORWARDED_TO_NODAL_OFFICER = 111,

        [Description("Forward to dealing hand for reconcile the fee")]
        FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE = 112,

        [Description("Raise Observations and Send to Applicant")]
        APP_OBJECTION_SCRUTINY_PHASE = 407,

        [Description("Objection Resolved & Application Re-Submitted")]
        OBJECTION_RESOLVED_APPLICATION_RESUBMITTED_SCRUTINY_PHASE = 408,

        [Description("Forwarded to nodal officer Labour Department")]
        FORWARDED_TO_NODAL_OFFICER_LABOUR_DEPT = 113,

        [Description("Forwarded for scrutiny complete")]
        FORWARDED_FOR_SCRUTINY_COMPLETE = 114,

        [Description("Forwarded for further action")]
        FORWARDED_FOR_FURTHER_ACTION = 103,

        [Description("Forwarded for further action ATP")]
        FORWARDED_FOR_FURTHER_ACTION_ATP = 115,

        [Description("Forwarded for further action CGM")]
        FORWARDED_FOR_FURTHER_ACTION_CGM = 116,

        [Description("Forwarded for further action JDM")]
        FORWARDED_FOR_FURTHER_ACTION_JDM = 117,

        [Description("Forwarded for further action Draftsman")]
        FORWARDED_FOR_FURTHER_ACTION_DRAFTSMAN = 118,

        [Description("Fee Raised in PSIEC")]
        FEE_RAISED_IN_PSIEC = 412,

        [Description("Forwarded for approval to Labour Dept")]
        FORWARDED_FOR_APPROVAL_LABOUR_DEPT = 119,

        [Description("Approved and forwarded for further action")]
        APPROVED_AND_FORWARDED_FOR_FURTHER_ACTION = 213,

        [Description("Application Dormant")]
        APPLICATION_DORMANT = 411,

        [Description("Forwarded to officer after fee reconciliation")]
        FORWARDED_FEE_RECONCILIATION = 120,

        [Description("Application Withdrawn by Applicant")]
        WITHDRAW_APPLICATION = 210,

        [Description("Deemed (Fee pending)")]
        DEEMED_WITH_FEE_PENDING = 214,

        [Description("Deemed (Fee raised)")]
        DEEMED_FEE_RAISED = 215,

        [Description("Deemed (Raised fee paid)")]
        DEEMED_RAISED_FEE_PAID = 216,

        [Description("Forwarded as the Factory Act not applicable")]
        FORWARDED_AS_FACTORY_ACT_NOT_APPLICABLE = 121,

        [Description("Raise Observations and Send to JDM in scrutiny phase")]
        RAISE_OBJECTION_SENT_TO_JDM_SCRUTINY_PHASE = 122,
    }

    public enum BusinessTypeEnum
    {
        [Description("Trade")]
        TRADE = 1,

        [Description("Industry")]
        INDUSTRY = 2,

        [Description("Manufacture or Occupation")]
        MANUFACTURE_OR_OCCUPATION = 3
    }

    public enum ApplicationPurposeTypeEnum
    {
        [Description("Default")]
        DEFAULT = 0,

        [Description("Grant Of Licence")]
        GRANT_LICENCE = 1,

        [Description("Renewal Of Licence")]
        RENEWAL_LICENCE = 2,

        [Description("Amendment Of Licence")]
        AMENDMENT_LICENCE = 3,

        [Description("LWB Fund Contribution")]
        LWB_CONTRIBUTION = 4,

        [Description("Unpaid Wages")]
        UNPAID_WAGES = 5

        // Building Plan enum

        //[Description("Proposed Building Plan")]
        //BP_PROPOSED_PLAN = 4,

        //[Description("Constructed Building With Stability")]
        //BP_CONSTRUCTED_WITH_STABILITY = 5,

        //[Description("Only Stability Certificate")]
        //BP_ONLY_STABILITY_CERTIFICATE = 6,

        //[Description("Additional Building Plan")]
        //BP_ADDITIONAL_CERTIFICATE = 7,

        //[Description("Stability Certificate After 5 Year")]
        //BP_STABILITY_CERTIFICATE_AFTER_FIVE_YEARS = 8,

        //[Description("Grant Of Licence with no Renewal")]
        //GRANT_LICENCE_WITH_NO_RENEWAL = 9,
    }

    public enum YesNoEnum
    {
        [Description("Yes")]
        YES = 1,

        [Description("No")]
        NO = 0
    }
    public enum PaymentGatewayTypeEnum
    {
        [Description("SBI")]
        SBI = 1,

        [Description("HDFC")]
        HDFC = 2,

        [Description("IFMS")]
        IFMS = 3,

        [Description("IFMS_NON_TREASURY")]
        IFMS_NON_TREASURY = 4
    }

    public enum PaymentModeTypeEnum
    {
        [Description("Online")]
        ONLINE = 1,

        [Description("Offline")]
        OFFLINE = 2
    }

    public enum PaymentTreasuryTypeEnum
    {
        [Description("Treasury")]
        TREASURY = 1,

        [Description("Non-Treasury")]
        NON_TREASURY = 2,

    }
    public enum PaymentGatewayApiMethodTypeEnum
    {
        [Description("Get")]
        GET = 1,

        [Description("Post")]
        POST = 2
    }

    public enum CircleTypeEnum
    {
        [Description("Factory Circle")]
        FACTORY_CIRCLE = 1,

        [Description("ALC Circle")]
        ALC_CIRCLE = 2,

        [Description("Labour Circle")]
        LABOUR_CIRCLE = 3,

        [Description("Join Director Circle")]
        JOINT_DIRECTOR_CIRCLE = 4,

        [Description("Medical Circle")]
        MEDICAL_CIRCLE = 5
    }

    public enum TransactionFinalStatusTypeEnum
    {
        [Description("Unlnown")]
        UNKNOWN = -1,

        [Description("Incomplete Payment Request")]
        NEW = 0,

        [Description("Succeed")]
        SUCCEED = 1,

        [Description("Failed")]
        FAILED = 2,

        [Description("Pending")]
        PENDING = 3
    }

    public enum DashboardRecordsHeaderTypeEnum
    {
        [Description("Inbox")]
        INBOX = 1,

        [Description("Recent-Processed")]
        SENT = 2,

        [Description("In-Processed")]
        IN_PROCESS = 3,

        [Description("Approved")]
        APPROVED = 4,

        [Description("Rejected")]
        REJECTED = 5,

        [Description("DEEMED")]
        DEEMED = 6,

        [Description("Pending At Dealing Hand")]
        PENDING_AT_DEALING_HAND = 7,

        [Description("Dormant")]
        DORMANT = 8,

        [Description("Withdrawn")]
        WITHDRAWN = 9

        //[Description("Deemed (fee pending")]
        //DEEMED_WITH_FEE_PENDING = 9,
    }

    public enum RoleRelationTypeEnum
    {
        [Description("Parent")]
        PARENT = 1,

        [Description("Child")]
        CHILD = 2
    }

    public enum NotificationModeTypeEnum
    {
        [Description("Mobile")]
        MOBILE = 1,

        [Description("Email")]
        EMAIL = 2,

        [Description("Mobile App")]
        MOBILE_APP = 3
    }

    public enum NotificationPurposeTypeEnum
    {
        [Description("OTP")]
        OTP = 1,

        [Description("Information")]
        INFO = 2
    }

    public enum NotificationStatusTypeEnum
    {
        [Description("NEW")]
        NEW = 0,

        [Description("Sent")]
        SENT = 1,

        [Description("Has error")]
        Has_Error = 2,

        [Description("Notofication server is disabled")]
        SERVER_DISABLED = 3
    }

    public enum ApplicationLifeCycleStatusTypeEnum
    {
        [Description("Not Submitted")]
        NOT_SUBMITTED = -1,

        [Description("In Process")]
        IN_PROCESS = 0,

        [Description("Approved")]
        APPROVED = 1,

        [Description("Rejected")]
        REJECTED = 2,

        [Description("In Objection")]
        IN_OBJECTION = 3,

        [Description("Deregistered")]
        DEREGISTERED = 4,

        [Description("Deemed Initialized")]
        DEEMED_INITIALIZED = 5,

        [Description("Deemed Completed")]
        DEEMED_COMPLETED = 6,

        [Description("Objection raise with balance fee")]
        IN_OBJECTION_WITH_BALANCE_FEE = 7,

        [Description("Auto Approvel Initialized")]
        AUTO_APPROVEL_INITIALIZED = 8,

        [Description("Auto Approvel Completed")]
        AUTO_APPROVEL_COMPLETED = 9,

        [Description("Acknowledgment Submitted Completed")]
        ACKNOWLEDGMENT_SUBMITTED_COMPLETED = 10,

        [Description("Application withdrawn by applicant")]
        WITHDRAW_APPLICATION = 11,

        [Description("Application dormant")]
        APPLICATION_DORMANT = 12,

        [Description("Application Declined")]
        APPLICATION_DECLINED = 13,

        [Description("Deemed with fee pending")]
        DEEMED_WITH_FEE_PENDING = 14
    }

    public enum BankTransactionReferenceTypeEnum
    {
        [Description("CIN")]
        CIN = 1,

        [Description("GRN")]
        GRN = 2,

        [Description("Bank Reference Number")]
        BANK_REF_NUMBER = 3
    }
    public enum BuidingPlanHUDApprovalTypeEnum
    {
        [Description("Regular Approval")]
        REGULAR_APPROVAL = 1

        //[Description("Compounding Approval")]
        //COMPOUNDING_APPROVAL = 2
    }

    public enum BuildingTypeEnum
    {
        [Description("Industry")]
        INDUSTRY = 1,

        [Description("IT Industry")]
        IT_INDUSTRY = 2,
    }
    public enum InspectionTypeEnum
    {
        //[Description("High Risk")]
        //HIGH_RISK = 1,

        //[Description("Medium Risk")]
        //MEDIUM_RISK = 2,

        //[Description("Low Risk")]
        //LOW_RISK = 3

        [Description("Highly Hazardous")]
        HIGH_RISK = 1,

        [Description("Moderately Hazardous")]
        MEDIUM_RISK = 2,

        [Description("Less Hazardous")]
        LOW_RISK = 3,

        [Description("Very Less Hazardous")]
        VERY_LOW_RISK = 4
    }

    public enum ProjectTypeEnum
    {
        //[Description("New")]
        //NEW = 1,

        [Description("Proposed")]
        PROPOSED = 2,

        [Description("Existing")]
        EXISTING = 3,

        [Description("Expansion")]
        EXPENSION = 4,

        [Description("Revised")]
        REVISED = 5,
    }

    public enum PartnerPortalTypesEnum
    {
        [Description("PbIndustry")]
        PB_INDUSTRY = 1,
    }

    public enum ShopTypeEnum
    {
        [Description("Shop")]
        SHOP = 1,

        [Description("Commercial Establishment")]
        COMMERCIAL_ESTABLISHMENT = 2,

        [Description("Registered Companies")]
        REGISTERED_COMPANIES = 3,

        [Description("Other")]
        OTHER = 4
    }

    public enum ClosingDayTypeEnum
    {
        [Description("Sunday")]
        SUNDAY = 1,

        [Description("Monday")]
        MONDAY = 2,

        [Description("Tuesday")]
        TUESDAY = 3,

        [Description("Wednesday")]
        WEDNESDAY = 4,

        [Description("Thursday")]
        THURSDAY = 5,

        [Description("Friday")]
        FRIDAY = 6,

        [Description("Saturday")]
        SATURDAY = 7,

        [Description("Not Applicable")]
        NOT_APPLICABLE = 8
    }
    public enum ClearenceTypeEnum
    {
        [Description("Registration")]
        REGISTRATION = 1,

        [Description("Renewal")]
        RENEWAL = 2,

        [Description("Amendment")]
        AMENDMENT = 3
    }

    public enum IndustryColorCodeByPPCBTypeEnum
    {
        [Description("Green")]
        REGISTRATION = 1,

        [Description("Orange")]
        RENEWAL = 2,

        [Description("Red")]
        AMENDMENT = 3
    }

    public enum UploadCsvErrorTypeEnum
    {
        [Description("All ok")]
        ALL_OK = 0,

        [Description("Column Mismatch")]
        COLUMN_MISMATCH = 1,

        [Description("Empty Sheet")]
        EMPTY_SHEET = 2,

        [Description("Data Validation")]
        DATA_VALIDATION = 3
    }

    public enum CsvColumnMismatchTypeEnum
    {
        [Description("MISSED")]
        MISSED = 1,

        [Description("EXTRA")]
        EXTRA = 2,
    }
    public enum DeemedProcessStatusTypeEnum
    {
        [Description("Pending")]
        PENDING = 0,

        [Description("Completed")]
        COMPLETED = 1,

        [Description("Error")]
        ERROR = 2,

        [Description("Completed by Officer")]
        COMPLETED_BY_OFFICER = 3,

        [Description("Completed With Fee Pending")]
        COMPLETED_WITH_FEE_PENDING = 4,

        [Description("Action by Officer")]
        ACTION_BY_OFFICER = 5
    }

    public enum AutoApproveProcessStatusTypeEnum
    {
        [Description("Pending")]
        PENDING = 0,

        [Description("Completed")]
        COMPLETED = 1,

        [Description("Error")]
        ERROR = 2
    }
    public enum DeemedProcessEngineTypeEnum
    {
        [Description("Shop")]
        SHOP = 1,

        [Description("Factory")]
        FACTORY = 2
    }

    public enum LWB_FundContributionAreaTypeEnum
    {
        [Description("Circle Wise")]
        CIRCLE_WISE = 1,

        [Description("Whole State")]
        WHOLE_STATE = 2
    }

    public enum LWB_FundContributionPaymentTypeEnum
    {
        [Description("Advance")]
        ADVANCE = 1,

        [Description("Balance")]
        BALANCE = 2,

        [Description("Complete")]
        COMPLETE = 3
    }

    public enum NationalityTypeEnum
    {
        [Description("Punjab")]
        PUNJAB = 0,

        [Description("India")]
        INDIA = 1,

        [Description("Non-Indian")]
        NON_INDIAN = 2
    }

    public enum FactoryHazardousCategoryTypeEnum
    {
        [Description("NA")]
        NOT_APPLICABLE = 0,

        [Description("A1")]
        A1 = 1,

        [Description("A2")]
        A2 = 2,

        [Description("A3")]
        A3 = 3,

        [Description("B")]
        B = 4,

        [Description("C")]
        C = 5,

        [Description("D")]
        D = 6,

    }
    public enum FactorySectionCategoryTypeEnum
    {
        [Description("NA")]
        NOT_APPLICABLE = 0,

        [Description("Section 2m(i)")]
        SECTION_2_M_I = 1,

        [Description("Section 2m(ii)")]
        SECTION_2_M_II = 2,

        [Description("Section 85")]
        SECTION_85 = 3,

        [Description("2(cb)")]
        SECTION_CB = 4,

        [Description("87(schedule)")]
        SECTION_87_SCHEDULE = 5


    }
    public enum FactorySessionCategoryTypeEnum
    {
        [Description("NA")]
        NOT_APPLICABLE = 0,

        [Description("Summer Season(Apr-Sep)")]
        SUMMER_SEASON_APR_SEP = 1,

        [Description("Winter Season(Oct-Mar)")]
        WINTER_SEASON_OCT_MAR = 2,

        [Description("Whole Year(Jan-Dec)")]
        WHOLE_YEAR_JAN_DEC = 3,

        [Description("Brick kiln Season(Jan-Jun)")]
        BRICK_KILN_SEASON_JAN_JUN = 4,
    }

    public enum AadharVerificationTypeEnum
    {
        [Description("Demographoic")]
        DEMOGRAPHIC = 1,

        [Description("Bio-Metric")]
        BIO_METRIC = 2,

        [Description("OTP")]
        OTP = 3
    }
    public enum InspectionStatusTypeEnum
    {
        [Description("Open")]
        OPEN = 0,

        [Description("Completed")]
        COMPLETED = 1
    }
    public enum InspectionFactoryExistenceTypeEnum
    {
        [Description("Open")]
        OPEN = 1,

        [Description("Temporary Closed")]
        TEMPORARY_CLOSED = 2,

        [Description("Seasonal Closed")]
        SEASONAL_CLOSED = 3,

        [Description("Permanent Closed")]
        PERMANENT_CLOSED = 4
    }

    public enum InspectionMusterRollTypeEnum
    {
        [Description("OnLicense")]
        ON_LICENSE = 1,

        [Description("OnInspectionTime")]
        On_INSPECTION_TIME = 2
    }

    public enum InspectionRandomizationProcessInitiatedByTypeEnum
    {
        [Description("By System")]
        BY_SYSTEM = 1,

        [Description("By User")]
        BY_USER = 2
    }
    public enum GeneralOptionTypeEnum
    {

        [Description("Not Applicable")]
        NOT_APPLICABLE = -1,

        [Description("No")]
        NO = 0,

        [Description("Yes")]
        YES = 1,
    }

    public enum NatureOfBusinessTypeEnum
    {
        [Description("Shop")]
        SHOP = 1,

        [Description("Establishment")]
        ESTABLISHMENT = 2
    }
    public enum WagesPaymentModeTypeEnum
    {

        [Description("Cash")]
        CASH = 1,

        [Description("Cheque")]
        CHEQUE = 2,

        [Description("ECS")]
        ECS = 3,
    }

    public enum InspectionEstablishmentTypeEnum
    {
        [Description("Factory")]
        FACTORY = 1,

        [Description("Rice Mill")]
        RICE_MILL = 2,

        [Description("Brick Kiln")]
        BRICK_KILN = 3,

        [Description("Ice Factory")]
        ICE_FACTORY = 4,

        [Description("Agricultural")]
        AGRICULTURAL = 5,
    }

    public enum ConstructionBuildingTypeEnum
    {
        [Description("Factory")]
        FACTORY = 1,

        [Description("Residential")]
        RESIDENTIAL = 2,

        [Description("Commercial")]
        COMMERCIAL = 3,

        [Description("Institutional")]
        INSTITUTIONAL = 4,

        [Description("Mixed Use (Residential & Commercial)")]
        MIXED_USE = 5,

        [Description("Any Other")]
        ANY_OTHER = 6,
    }
    public enum BocwEngagedWorkerSlabTypeEnum
    {
        [Description("Below 10")]
        BELOW_10 = 1,

        [Description("Above 10")]
        ABOVE_10 = 2
    }
    public enum BocwRegisteredTypeEnum
    {
        [Description("Principal Employer")]
        PRINCIPAL_EMPLOYER = 1,

        [Description("Contractor")]
        CONTRACTOR = 2
    }
    public enum EngagedAnyContractorTypeEnum
    {
        [Description("Yes")]
        YES = 1,

        [Description("No")]
        NO = 2
    }
    public enum BOCWActCircleTypeEnum
    {
        [Description("Factory Wing")]
        FACTORY_WING = 1,

        [Description("Labour Wing")]
        LABOUR_WING = 2
    }

    public enum BuildingPlanApprovalAuthorityTypeEnum
    {

        [Description("Competent Person")]
        COMPETENT_PERSON = 1,

        [Description("Empaneled Architect")]
        EMPANELED_ARCHITECT = 2,

        [Description("Empaneled Engineer")]
        EMPANELED_ENGINEER = 3
    }

    public enum BuildingPlanStabilityAuthorityTypeEnum
    {

        [Description("Competent Person")]
        COMPETENT_PERSON = 1,

        [Description("Invest Punjab")]
        INVEST_PUNJAB = 2,

        [Description("Empaneled Engineer")]
        EMPANELED_ENGINEER = 3,

        [Description("Director Of Factories")]
        DIRECTOR_FACTORIES = 4,

        [Description("Offline")]
        OFFLINE = 5

    }

    public enum MotorTransportNameAddressTypeEnum
    {
        [Description("Proprietor and partners of motor transport undertaking in case of a firm not registered under the companies act, 1956")]
        PROPRIETOR_PARTNERS = 1,

        [Description("General Manager in case of a public sector undertaking")]
        GENERAL_MANAGER = 2
    }

    public enum InspectionViolationExistTypeEnum
    {
        [Description("No Violation")]
        HAS_NO_VIOLATION = 0,

        [Description("Violation")]
        HAS_VIOLATION = 1,
    }

    public enum ExistingBuildingPlanTypeEnum
    {
        [Description("Addition")]
        ADDITION = 1,

        [Description("Amendment")]
        AMENDMENT = 2,

        [Description("Addition/Amendment")]
        ADDITION_AMENDMENT = 3,
    }

    public enum ToDoCodeTypeEnum
    {
        [Description("Pending")]
        PENDING = 0,

        [Description("Initiate Application")]
        INITIATE_APPLICATION = 1,

        [Description("Entry in Master Table")]
        MASTER_ENTRY_NEW = 2,


        [Description("Initiate Application Action")]
        INITIATE_APPLICATION_ACTION = 3,

        [Description("Share Status With Invest Punjab")]
        SHARE_STATUS_WITH_INVEST_PUNJAB = 4,

        [Description("Update CircleRefId in Project Site")]
        UPDATE_CIRCLEID_IN_PROJECT_SITE = 5,

        [Description("Update NativeAppRefId in BF_RequestLogTable")]

        UPDATE_NATIVE_APPID_IN_BF_REQLOG = 6,

        [Description("Update Master Table")]
        MASTER_ENTRY_UPDATE = 7,

        [Description("Update Last Modified Date In Application Table")]
        UPDATE_LAST_MODIFIED_DATE_IN_APPLICATION = 8,

        [Description("Set Application Dirty Flag")]
        SET_ISDIRTY_FLAG = 9,

        [Description("Set Lock Application")]
        SET_LOCK_APPLICATION = 10,

        [Description("Set Application Life Cycle")]
        SET_APPLICATION_LIFECYCLE = 11,

        [Description("Send Application To Landing officer")]
        SEND_APPLICATION_TO_LANDING_OFFICER = 12,

        [Description("Send Application To Landing Officer After Objection Resolved")]
        SEND_APPLICATION_TO_LANDING_OFFICER_OBJECTION_RESOLVED = 13,

        [Description("Set IsHavingEmployee Flag")]
        SET_ISHAVING_EMPLOYEE_FLAG = 14,

        [Description("Set Registration/Renewal/Amendment Dates")]
        SET_REG_REN_AMD_DATES = 16,

        [Description("Set Amendment History Counter")]
        SET_AMENDMENT_HISTORY_COUNTER = 17,

        [Description("Prepare Data As Per Temporary Registration Flag")]
        PREPARE_DATA_AS_PER_TEMP_REG_FLAG = 18,

        [Description("Prepare And Set Amendment History Data")]
        PREPARE_AND_SET_AMENDMENT_HISTORY_DATA = 19,

        [Description("Set Is Fee Applicable Flag")]
        SET_IS_FEE_APPLICABLE_FLAG = 20,

        [Description("Send Notification")]
        SEND_NOTIFICATION = 21,

        [Description("Add Payment Raised Fee")]
        ADD_PAYMENT_RAISED_FEE = 22,

        [Description("Increase Payment Batch Counter")]
        INCREASE_PAYMENT_BATCH_COUNTER = 23,

        [Description("Set Action Log Id")]
        SET_ACTION_LOG_ID = 24,

        [Description("Update Payment Raised Fee")]
        UPDATE_PAYMENT_RAISED_FEE = 25,

        [Description("Withdraw Application")]
        WITHDRAW_APPLICATION = 26,

        [Description("Seed Time Line Flow Data")]
        SEED_TIME_LINE_FLOW_DATA = 27,

        [Description("Application form sub part save (Trade Union Add Officer)")]
        APPLICATION_FORM_SUB_PART_SAVE_TRADE_UNION_ADD_OFFICER = 8001,

        [Description("Application form sub part save (Trade Union Officer)")]
        APPLICATION_FORM_SUB_PART_SAVE_TRADE_UNION_OFFICER = 8002,
    }

    public enum ToDoActivityModeTypeEnum
    {
        [Description("Default")]
        DEFAULT = 0,

        [Description("Add New Mode")]
        ADD_NEW_MODE = 1,

        [Description("Edit Mode")]
        EDIT_MODE = 2,

        [Description("Resolve Objection Mode")]
        RESOLVE_OBJECTION_MODE = 3,

        [Description("Lock Application Mode")]
        LOCK_APPLICATION_MODE = 4,

        [Description("Fee Raised By Officer")]
        FEE_RAISED_BY_OFFICER = 5,

        [Description("Withdraw Application Mode")]
        WITHDRAW_APPLICATION_MODE = 6,

        [Description("Resolve Objection WIth Balance Fee Mode")]
        RESOLVE_OBJECTION_BALANCE_FEE_MODE = 7,
    }
    public enum ToDoActivityCompleteTypeEnum
    {
        [Description("Pending")]
        PENDING = 0,

        [Description("Completed Succesfully")]
        SUCCEEDED = 1,

        [Description("Failed with Errors")]
        FAILED_WITH_ERRORS = 2,

        [Description("Failed due to required parameters missing")]
        FAILED_MISSING_PARAMETERS = 3,

        [Description("Skipped")]
        SKIPPED = 4
    }

    public enum ToDoActivityCategoryTypeEnum
    {
        [Description("Default")]
        DEFAULT = 0,

        [Description("Application form main part save")]
        APPLICATION_FORM_MAIN_PART_SAVE = 1,

        [Description("Application form lock")]
        APPLICATION_FORM_LOCK = 3,

        [Description("Application fee pay")]
        APPLICATION_FEE_PAY = 4,

        [Description("Document upload")]
        DOCUMENT_UPLOAD = 5,

        [Description("Fee Calculation")]
        FEE_CALCULATION = 6,

        [Description("Application form sub part save (Shop Employee)")]
        APPLICATION_FORM_SUB_PART_SAVE_SHOP_EMPLOYEE = 6001,

        [Description("Application form sub part save (Factory Occupier, Manager Detail)")]
        APPLICATION_FORM_SUB_PART_SAVE_FCATORY_OCCUPIER_MANAGER = 7001,

        [Description("Withdraw Application")]
        WITHDRAW_APPLICATION = 7,

        [Description("Application form sub part save (Add Contractor)")]
        APPLICATION_FORM_SUB_PART_SAVE_PRINCIPAL_EMPLOYER_ADD_CONTRACTOR = 8001,

        [Description("Application form sub part save")]
        APPLICATION_FORM_SUB_PART_SAVE_PRINCIPAL_EMPLOYER = 8002,


        [Description("Application form sub part save (OSH form 1 employee)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_FORM_1_REGISTRATION_EMPLOYEE = 1012,

        [Description("Application form sub part save (OSH form 1 factory)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_FORM_1_REGISTRATION_FACTORY = 1013,

        [Description("Application form sub part save (OSH form 1 bocw)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_FORM_1_REGISTRATION_BOCW = 1014,

        [Description("Application form sub part save (OSH form 1 motor transport)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_FORM_1_REGISTRATION_MOTOR_TRANSPORT = 1015,

        [Description("Application form sub part save (OSH form 1 epfo & esic)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_FORM_1_REGISTRATION_EPFO_ESIC = 1016,

        [Description("Application form sub part save (OSH form 1 employer)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_FORM_1_REGISTRATION_EMPLOYER = 1017,

        [Description("Application form sub part save (OSH form 1 employer)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_FORM_1_REGISTRATION_PRINCIPAL_EMPLOYER = 1018,

        [Description("Application form sub part save (OSH form 1 contractor)")]
        APPLICATION_FORM_SUB_PART_SAVE_OSH_Form_1_Registration_CONTRACTOR = 1019,

        #region For Samadhaan
        [Description("Application form sub part save (Individual Complaint form Employer/Contractor Details)")]
        APPLICATION_FORM_SUB_PART_SAVE_EMPLOYER_OR_CONTRACTOR_DETAILS = 2001,

        [Description("Application form sub part save (Individual Complaint form Workplace Details)")]
        APPLICATION_FORM_SUB_PART_SAVE_WORKPLACE_DETAILS = 2002,

        [Description("Application form sub part save (Individual Complaint form Establishment Details)")]
        APPLICATION_FORM_SUB_PART_SAVE_ESTABLISHMENT_DETAILS = 2003,


        [Description("Application form sub part save (Individual Complaint form Gratuity form )")]
        APPLICATION_FORM_SUB_PART_SAVE_GRATUITY_CLAIMS = 2005,

        [Description("Application form sub part save (Individual Complaint Code On Wages form )")]
        APPLICATION_FORM_SUB_PART_SAVE_CODE_ON_WAGES = 2006,

        [Description("Application form sub part save (Individual Minimum Wages Period And Amount form )")]
        APPLICATION_FORM_SUB_PART_SAVE_MINIMUM_WAGES_PERIOD_AMOUNT = 2007,

        [Description("Application form sub part save (Individual Minimum Wages form )")]
        APPLICATION_FORM_SUB_PART_SAVE_MINIMUM_WAGES = 2008,

        [Description("Application form sub part save (Individual Claim wages not paid for on weekly day of rest )")]
        APPLICATION_FORM_SUB_PART_SAVE_WEEKLY_DAY_OF_REST_WAGES = 2009,

        [Description("Application form sub part save (Individual Claim wages not paid for on weekly day of rest Period Amount )")]
        APPLICATION_FORM_SUB_PART_SAVE_WEEKLY_DAY_OF_REST_PERIOD_AMT_WAGES = 2010,

        #endregion
    }

    public enum ToDoTicketStatusTypeEnum
    {
        [Description("Open")]
        OPEN = 0,

        [Description("Closed")]
        CLOSED = 1,

        [Description("Re-open")]
        REOPEN = 2,
    }
    public enum SeedStatusTypeEnum
    {
        [Description("No Status")]
        NO_STATUS = 0,

        [Description("Seeded")]
        SEEDED = 1,

        [Description("No Seeding Required")]
        NO_SEEDING_REQUIRED = 2,

        [Description("No Data Available")]
        NO_DATA_AVAILABLE = 3,

        [Description("Error")]
        ERROR = 4
    }

    public enum FactoryCategoryTypeEnum
    {
        [Description("No Category")]
        NO_CATEGORY = 0,

        [Description("Category A")]
        CATEGORY_A = 1,

        [Description("Category B")]
        CATEGORY_B = 2,

        [Description("Category C")]
        CATEGORY_C = 3,

        [Description("Category D")]
        CATEGORY_D = 4,

    }
    //public enum ToDoTableTypeEnum
    //{
    //    [Description("Application")]
    //    APPLICATION = 1,

    //    [Description("ShopLicence_GeneralDetail")]
    //    SHOP_LICENCE_GENERAL_DETAIL = 2
    //}

    public enum PendencyTypeEnum
    {

        [Description("Pending At Department")]
        PENDING_AT_DEPARTMENT = 1,

        [Description("Pending At Investor")]
        PENDING_AT_INVESTOR = 2,
    }


    public enum ApplicationProcessPhaseLogTypeEnum
    {
        [Description("Scrutiny Phase")]
        SCRUTINY_PHASE = 1,

        [Description("Observation Phase")]
        OBSERVATION_PHASE = 2,

        [Description("Clarification Phase")]
        CLARIFICATION_PHASE = 3,

        [Description("Deemed Phase")]
        DEEMED_PHASE = 4
    }

    public enum SlabDayTypeEnum
    {
        [Description("Working")]
        WORKING = 0,

        [Description("Week End - Saturday")]
        WEEKEND_SATURDAY = 1,
        [Description("Week End - Sunday")]
        WEEKEND_SUNDAY = 2,

        [Description("Holiday")]
        HOLIDAY = 3
    }

    public enum PSIECPhaseTypeEnum
    {
        [Description("IFP")]
        IFP = 1,

        [Description("IGC")]
        IGC = 2,

        [Description("IPD")]
        IPD = 3,
    }

    public enum DepartmentCodeTypeEnum
    {
        [Description("Department of Labour")]
        LABOUR = 1,

        [Description("Department of PSIEC")]
        PSIEC = 3,
    }
    public enum DormantProcessStatusTypeEnum
    {
        [Description("Pending")]
        PENDING = 0,

        [Description("Completed")]
        COMPLETED = 1,

        [Description("Error")]
        ERROR = 2,

        [Description("Notification Sent")]
        NOTIFICATION_SENT = 3,

        [Description("Notification Waiting Time")]
        NOTIFICATION_WAITING_TIME = 4
    }
    public enum ActionTakenModeTypeEnum
    {
        [Description("User")]
        USER = 0,

        [Description("System")]
        SYSTEM = 1,
    }


    public enum RandomizationInitilaztionProcessStatusTypeEnum
    {
        [Description("In Process")]
        IN_PROCESS = 0,

        [Description("Successed")]
        SUCCESSED = 1,

        [Description("Failed")]
        FAILED = 2
    }
    public enum ActionPendingWithTypeEnum
    {
        [Description("Pending With Applicant")]
        APPLICANT_SIDE = 1,

        [Description("Pending With Department")]
        DEPARTMENT_SIDE = 2,
    }
    public enum ActionCategoryTypeEnum
    {
        [Description("Not Submitted")]
        NOT_SUBMITTED = 0,

        [Description("In-Process")]
        INPROCESS = 1,

        [Description("Observation")]
        OBSERVATION = 2,

        [Description("Fee Raised")]
        FEE_RAISED = 3,

        [Description("Disposed")]
        DISPOSED = 4,

    }

    public enum ApplicationActivationStatusTypeEnum
    {
        [Description("De-Activated")]
        DEACTIVATED = 0,

        [Description("Activated")]
        ACTIVATED = 1,

    }


    #region OSH Type Enums

    public enum OSH_EstablishmentTypeEnum
    {
        [Description("Factory for the purpose of contract labour")]
        [CustomeAttribute_EnumShortDesc("Registration of establishment")]
        OSH_FACTORY = 1,

        [Description("Building and Other Construction Work")]
        [CustomeAttribute_EnumShortDesc("BOCW")]
        OSH_BOCW = 2,

        [Description("Motor Transport")]
        [CustomeAttribute_EnumShortDesc("Motor Transport")]
        OSH_MOTOR_TRANSPORT = 3,

        [Description("Any other Establishment")]
        [CustomeAttribute_EnumShortDesc("Other Establishment")]
        OSH_ANY_OTHER_ESTABLISHMENT = 4,

        [Description("Mines")]
        [CustomeAttribute_EnumShortDesc("Mines")]
        OSH_MINES = 5,

        [Description("Dock Work")]
        [CustomeAttribute_EnumShortDesc("Dock Work")]
        OSH_DOCK_WORK = 6,
    }

    public enum OwnershipTypeEnum
    {
        [Description("Autonomous / Statutory Organisations")]
        [CustomeAttribute_EnumShortDesc("Autonomous / Statutory Organisations")]
        AUTONOMOUS_STATUTORY_ORGANISATIONS = 1,

        [Description("Central Govt. Offices/Department")]
        [CustomeAttribute_EnumShortDesc("Central Govt. Offices/Department")]
        CENTRAL_GOVT_OFFICES_DEPARTMENT = 2,

        [Description("Central PSU")]
        [CustomeAttribute_EnumShortDesc("Central PSU")]
        CENTRAL_PSU = 3,

        [Description("Co-operative Society")]
        [CustomeAttribute_EnumShortDesc("Co-operative Society")]
        CO_OPERATIVE_SOCIETY = 4,

        [Description("Establishment Type 1")]
        [CustomeAttribute_EnumShortDesc("Establishment Type 1")]
        ESTABLISHMENT_TYPE_1 = 5,

        [Description("Establishment Type 2")]
        [CustomeAttribute_EnumShortDesc("Establishment Type 2")]
        ESTABLISHMENT_TYPE_2 = 6,

        [Description("HUF")]
        [CustomeAttribute_EnumShortDesc("HUF")]
        HUF = 7,

        [Description("Individual Firm")]
        [CustomeAttribute_EnumShortDesc("Individual Firm")]
        INDIVIDUAL_FIRM = 8,

        [Description("Joint Venture")]
        [CustomeAttribute_EnumShortDesc("Joint Venture")]
        JOINT_VENTURE = 9,

        [Description("Limited Liability Partnership")]
        [CustomeAttribute_EnumShortDesc("Limited Liability Partnership")]
        LIMITED_LIABILITY_PARTNERSHIP = 10,

        [Description("Private Ltd. Company")]
        [CustomeAttribute_EnumShortDesc("Private Ltd. Company")]
        PRIVATE_LTD_COMPANY = 11,

        [Description("Partnership Firm")]
        [CustomeAttribute_EnumShortDesc("Partnership Firm")]
        PARTNERSHIP_FIRM = 12,

        [Description("Proprietorship Firm")]
        [CustomeAttribute_EnumShortDesc("Proprietorship Firm")]
        PROPRIETORSHIP_FIRM = 13,

        [Description("Registered Society")]
        [CustomeAttribute_EnumShortDesc("Registered Society")]
        REGISTERED_SOCIETY = 14,

        [Description("State Govt. Offices/Department")]
        [CustomeAttribute_EnumShortDesc("State Govt. Offices/Department")]
        STATE_GOVT_OFFICES_DEPARTMENT = 15,

        [Description("State PSU")]
        [CustomeAttribute_EnumShortDesc("State PSU")]
        STATE_PSU = 16,

        [Description("Trust")]
        [CustomeAttribute_EnumShortDesc("Trust")]
        TRUST = 17,

        [Description("Others")]
        [CustomeAttribute_EnumShortDesc("Others")]
        OTHERS = 18
    }

    public enum FactoryTypeEnum
    {
        [Description("Hazardous")]
        HAZARDOUS = 1,

        [Description("Non-Hazardous")]
        NON_HAZARDOUS = 2,

    }

    #endregion OSH Type Enums

    public enum OfflineReportStatusTypeEnum
    {
        [Description("Completed")]
        COMPLETED = 1,

        [Description("In-Process")]
        INPROCESS = 0,

        [Description("Failed")]
        FAILED = 3,

    }


    public enum LWBSlabTypeEnum
    {
        [Description("APR-MAR")]
        APR_MAR = 1,

        [Description("OCT-MAR")]
        OCT_MAR = 2,

        [Description("APR-MAR")]
        APR_SEP = 3,

        [Description("MAY_JUL")]
        MAY_JUL = 4,

        [Description("AUG-OCT")]
        AUG_OCT = 5,

        [Description("APR-MAR")]
        NOV_JAN = 6,

        [Description("APR-MAR")]
        FEB_APR = 7

    }

    public enum LWBAadhaarverificationStatusTypeEnum
    {
        [Description("Pending Model Validation")]
        PENDING_MODEL_VALIDATION = 0,

        [Description("Pending Aadhar Validation")]
        PENDING_AADHAR_VALIDATION = 1,

        [Description("In-Process")]
        IN_PROCESS = 2,

        [Description("Completed With Success")]
        COMPLETED_WITH_SUCCESS = 3,

        [Description("Completed With Errors")]
        COMPLETED_WITH_ERRORS = 4,

    }

    public enum AadharVerificationDemographicTypeEnum
    {
        [Description("Pending")]
        PENDING = 1,

        [Description("Valid")]
        VALID = 2,

        [Description("Invalid")]
        INVALID = 3
    }

    public enum AadharVerificationFormatTypeEnum
    {
        [Description("Pending")]
        PENDING = 1,

        [Description("Valid")]
        VALID = 2,

        [Description("Invalid")]
        INVALID = 3
    }

    public enum LeaveTypeEnum
    {
        [Description("Present")]
        PRESENT = 0,

        [Description("On Leave")]
        ONLEAVE = 1,

    }



    public enum User2FactorVerificationTypeEnum
    {
        [Description("Pending")]
        PENDING = 0,

        [Description("Otp Verified")]
        OTP_VERIFIED = 1,

        [Description("Disposed")]
        DISPOSED = 2
    }

    #region For samadhaan portal
    public enum ComplaintCategoryTypeEnum
    {
        [Description("Payment Related")]
        PAYMENT = 1,

        [Description("Appeal")]
        APPEAL = 2,

        [Description("Job Related")]
        JOB = 3,

        [Description("Penalities_Composition Related")]
        PENALITIES_COMPOSITION = 4,
    }

    public enum WorkerCategoryTypeEnum
    {
        [Description("Administrative")]
        ADMINISTRATIVE = 1,

        [Description("Clerical")]
        CLERICAL = 2,

        [Description("Highly Skilled")]
        HIGHLY = 3,

        [Description("Managerial")]
        MANAGERIAL = 4,

        [Description("Semi Skilled")]
        SEMI_SKILLED = 5,

        [Description("Skilled")]
        SKILLED = 6,

        [Description("Supervisor")]
        SUPERVISOR = 7,

        [Description("Unskilled")]
        UNSKILLED = 8,
    }

    public enum WagePeriodtypeEnum
    {
        [Description("Daily")]
        DAILY = 1,

        [Description("Weekly")]
        WEEKLY = 2,

        [Description("Fortnightly")]
        FORTNIGHTLY = 3,

        [Description("Monthly")]
        MONTHLY = 4,
    }

    public enum GratuityClaimBasisTypeEnum
    {
        [Description("Superannuation")]
        SUPERANNUATION = 1,

        [Description("Retirement or Resignation")]
        RETIREMENT_OR_RESIGNATION = 2,

        [Description("Termination of Contract under fixed term employment")]
        TERMINATION_OF_CONTRACT_UNDER_FIXED_TERM_EMPLOYMENT = 3,

        [Description("Termination for any other reason")]
        TERMINATION_FOR_ANY_OTHER_REASON = 4
    }

    public enum MaritalStatusTypeEnum
    {
        [Description("Unmarried")]
        UNMARRIED = 1,

        [Description("Married")]
        MARRIED = 2,

        [Description("Widowed")]
        WIDOWED = 3,

        [Description("Divorced")]
        DIVORCED = 4,

        [Description("Separated")]
        SEPARATED = 5
    }
    public enum MaternityDischargeTypeEnum
    {
        [Description("Discharged/Dismissal")]
        DischargedDismissal = 1,

        [Description("Change in service Conditions")]
        ChangeInServiceConditions = 2
    }

    public enum MoneyDueReasonTypeEnum
    {
        [Description("Settlement")]
        SETTLEMENT = 1,

        [Description("Award")]
        AWARD = 2,

        [Description("Notice Pay")]
        NOTICE_PAY = 3,

        [Description("Retrenchment/Closure Compensation")]
        RETRENCHMENT_CLOSURE_COMPENSATION = 4,

        [Description("Lay off Compensation")]
        LAY_OFF_COMPENSATION = 5
    }


    public enum SettlementTypeEnum
    {
        [Description("Bipartite")]
        BIPARTITE = 1,

        [Description("Tripartite")]
        TRIPARTITE = 2
    }

    public enum NoticePayPeriodTypeEnum
    {
        [Description("One Month")]
        ONE_MONTH = 1,

        [Description("Two Months")]
        TWO_MONTHS = 2,

        [Description("Three Months")]
        THREE_MONTHS = 3
    }

    public enum AllowanceTypeEnum
    {
        [Description("Extra Wages For Underground Allowance")]
        EXTRA_WAGES_FOR_UNDERGROUND_ALLOWANCE = 1,

        [Description("Height Allowance")]
        HEIGHT_ALLOWANCE = 2,

        [Description("Tunnel Allowance")]
        TUNNEL_ALLOWANCE = 3,

        [Description("Winter Allowance")]
        WINTER_ALLOWANCE = 4
    }

    public enum PlaceOfWorkTypeEnum
    {
        [Description("Chandigarh")]
        CHANDIGARH = 1,

        [Description("Punjab")]
        PUNJAB = 2,
    }

    public enum BonusClaimTypeEnum
    {
        [Description("Statutory minimum bonus at 8.33%")]
        Statutory_minimum_bonus_at_8 = 1,

        [Description("Bonus beyond the statutory 8.33%")]
        Bonus_beyond_the_statutory_8 = 2,
    }

    public enum SamadhaanEstablishmentTypeEnum
    {
        [Description("Airports / Airlines / Air Transport Services and its contractor")]
        Airports_Airlines_Air_Transport_Services_and_its_contractor = 1,

        [Description("Any Boards / Corporations of the Central Government and its contractor")]
        Any_Boards_Corporations_of_the_Central_Government_and_its_contractor = 2,

        [Description("Any Controlled Industry, declared so by the Central Government")]
        Any_Controlled_Industry_declared_so_by_the_Central_Government = 3,

        [Description("Any establishment of the State Public Sector engaged in Mining / Oil & Gas activity and its contractor")]
        Any_establishment_of_the_State_Public_Sector_engaged_in_Mining_Oil_Gas_activity_and_its_contractor = 4,

        [Description("Any office of the Central Government and its contractor")]
        Any_office_of_the_Central_Government_and_its_contractor = 5,

        [Description("Any other establishment under Central jurisdiction, not covered above and its contractor")]
        Any_other_establishment_under_Central_jurisdiction_not_covered_above_and_its_contractor = 6,

        [Description("Any other establishment, being funded by the Central Government and its contractor")]
        Any_other_establishment_being_funded_by_the_Central_Government_and_its_contractor = 7,

        [Description("Any State PSU engaged as Contractor of Central Government establishment and its contractor")]
        Any_State_PSU_engaged_as_Contractor_of_Central_Government_establishment_and_its_contractor = 8,

        [Description("Bank & Insurance and its contractor")]
        Bank_Insurance_and_its_contractor = 9,

        [Description("Cement Industry and its contractor")]
        Cement_Industry_and_its_contractor = 10,

        [Description("Central Government Autonomous Bodies and its contractor")]
        Central_Government_Autonomous_Bodies_and_its_contractor = 11,

        [Description("Central Government Institutes / Hospitals and its contractor")]
        Central_Government_Institutes_Hospitals_and_its_contractor = 12,

        [Description("Central Government Research Institutes and its contractor")]
        Central_Government_Research_Institutes_and_its_contractor = 13,

        [Description("Central Public Sector Undertaking / Enterprises / Establishment")]
        Central_Public_Sector_Undertaking_Enterprises_Establishment = 14,

        [Description("Central Regulatory Bodies / Commissions and its contractor")]
        Central_Regulatory_Bodies_Commissions_and_its_contractor = 15,

        [Description("Central Universities and its contractor")]
        Central_Universities_and_its_contractor = 16,

        [Description("Co-operative Banks and its contractor")]
        Co_operative_Banks_and_its_contractor = 17,

        [Description("Coal Industry and its contractor")]
        Coal_Industry_and_its_contractor = 18,

        [Description("Defence establishments / Cantonment Boards and its contractor")]
        Defence_establishments_Cantonment_Boards_and_its_contractor = 19,

        [Description("Defence PSU.Central Government Establishments constituted by the Central Act (e.g. NHAI, FCI, CWC etc) and its contractor")]
        Defence_PSU_Central_Government_Establishments_constituted_by_the_Central_Act_and_its_contractor = 20,

        [Description("Establishment having department or branches in more than one state for the purpose of gratuity & maternity benefit only")]
        Establishment_having_department_or_branches_in_more_than_one_state_for_the_purpose_of_gratuity_maternity_benefit_only = 21,

        [Description("Major Ports and its contractor")]
        Major_Ports_and_its_contractor = 22,

        [Description("Mining (In relation to Mining activity or otherwise) and its contractor")]
        Mining_In_relation_to_Mining_activity_or_otherwise_and_its_contractor = 23,

        [Description("Non Coal Mines & Minerals and its contractor")]
        Non_Coal_Mines_Minerals_and_its_contractor = 24,

        [Description("Oil & Gas / Pipelines / Oilfields / Oil refinery / Oil & Gas Companies and its contractor")]
        Oil_Gas_Pipelines_Oilfields_Oil_refinery_Oil_Gas_Companies_and_its_contractor = 25,

        [Description("Quarry (e.g. Stone Mines) and its contractor")]
        Quarry_e_g_Stone_Mines_and_its_contractor = 26,

        [Description("Railways / Metro Railways / Railway Company / Railway Factory and its contractor")]
        Railways_Metro_Railways_Railway_Company_Railway_Factory_and_its_contractor = 27,

        [Description("Telecom / Internet Services Provider (e.g. BSNL, Airtel, Jio)")]
        Telecom_Internet_Services_Provider_e_g_BSNL_Airtel_Jio = 28,
    }

    public enum OrderNumTypeEnum
    {
        [Description("Statutory minimum bonus at 8.33%")]
        ORDER_NUM = 1,

    }


    #endregion

}
