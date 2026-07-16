using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class ApplicationLevelCommonOps
    {
        public static ServiceCodeApplicationTypeMapperViewModel FindServiceCodeByApplicationType_PurposeType(ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            List<ServiceCodeApplicationTypeMapperViewModel> serviceCodeApplicationTypeMapper = new List<ServiceCodeApplicationTypeMapperViewModel>();

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 6, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 7, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 8, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 5, ApplicationType = ApplicationTypeEnum.SHOP_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 15, ApplicationType = ApplicationTypeEnum.SHOP_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 4, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 12, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 13, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });


            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 35, ApplicationType = ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 36, ApplicationType = ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 17, ApplicationType = ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 18, ApplicationType = ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 22, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 23, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 24, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 63, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_HUD, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 61, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 61, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 76, ApplicationType = ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 62, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 62, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 76, ApplicationType = ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 71, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 72, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_EXISTING, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 73, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 81, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_PSIEC, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 3, ApplicationType = ApplicationTypeEnum.PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 11, ApplicationType = ApplicationTypeEnum.PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 19, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 20, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 21, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 101, ApplicationType = ApplicationTypeEnum.OSH_FORM_1_Registration, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 101, ApplicationType = ApplicationTypeEnum.OSH_FORM_1_Registration, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 101, ApplicationType = ApplicationTypeEnum.OSH_FORM_1_Registration, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            return serviceCodeApplicationTypeMapper.Where(x => x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType).FirstOrDefault();
        }
    }
}
