using System.Runtime.CompilerServices;
using AutoMapper;
using MiddlewareService.Domain.Entities;
using MiddlewareService.Domain.Interfaces;
using MiddlewareService.Domain.Models.Request;
using MiddlewareService.Domain.Models.Response;

namespace MiddlewareService.Application.Services{
    public class ApiService{
        private readonly IAuthenticationService _authenticationService;
        private readonly ISearchService _searchService;
        private readonly IUnnaxService _unnaxService;
        private readonly IMapper _mapper;
        public ApiService(IAuthenticationService authenticationService, ISearchService searchService, IUnnaxService unnaxService, IMapper mapper){
            _authenticationService = authenticationService;
            _searchService = searchService;
            _unnaxService = unnaxService;
            _mapper = mapper;
        }

        public async Task<ResponseBase<List<Province>>> GetProvinces(FilterRequest request) 
        {
            string usr = "ffldev.admin";
            string password = "24^lD5f%DG)5";
            var token = await _authenticationService.GetTokenAsync(usr,password);
            var provinceDto = await _searchService.GetProvincesAsync(token, request);

            var mappedProvinces = _mapper.Map<List<Province>>(provinceDto.Data);

            var response = new ResponseBase<List<Province>>()
            {
                Code = provinceDto.Code,
                Message = provinceDto.Message,
                Data = mappedProvinces
            };
            return response;   
        }

        public async Task<ResponseBase<List<Bank>>> GetBanks(FilterRequest request) 
        {
            string usr = "ffldev.admin";
            string password = "24^lD5f%DG)5";
            var token = await _authenticationService.GetTokenAsync(usr,password);
            var bankDto = await _searchService.GetBanksAsync(token, request);

            var mappedBanks = _mapper.Map<List<Bank>>(bankDto.Data);

            var response = new ResponseBase<List<Bank>>()
            {
                Code = bankDto.Code,
                Message = bankDto.Message,
                Data = mappedBanks
            };

            return response;   
        }

        public async Task<ResponseBase<Unnax>> InsertBank(BankRequest request) 
        {
            _ = new ResponseBase<Unnax>();
            string usr = "ffldev.admin";
            string password = "24^lD5f%DG)5";
            var token = await _authenticationService.GetTokenAsync(usr,password);
            var Id = await _unnaxService.Insert(token, request);
            ResponseBase<Unnax> response = await _unnaxService.LoginAsync(token, Id);
            return response;   
        }

        public async Task<ResponseBase<List<BankMovement>>> GetBankMovement(FilterRequest request) 
        {
            string usr = "ffl1.president@vtortosa.nom.es";
            string password = "FFL.1234";
            var token = await _authenticationService.GetTokenAsync(usr,password);
            var bankDto = await _searchService.GetBankMovementAsync(token, request);

            var mappedBanks = _mapper.Map<List<BankMovement>>(bankDto.Data);

            var orderedMovements = mappedBanks.OrderByDescending(m => m.MovementId).ToList();
            decimal currentTotal = orderedMovements.First().Total;
            var calculatedMovements = mappedBanks
            .OrderBy(m => m.MovementId)
            .Select(m => {
            currentTotal += m.Amount;
             return new BankMovement {
                MovementId = m.MovementId,
                Date = m.Date,
                Concept = m.Concept,
                Amount = m.Amount,
                Total = currentTotal,
                Icon = ObtenerIcono(m.Concept)
             };
            })
            .ToList();

            var response = new ResponseBase<List<BankMovement>>()
            {
                Code = bankDto.Code,
                Message = bankDto.Message,
                Data = calculatedMovements
            };

            return response;   
        }

        public static string ObtenerIcono(string concepto)
{
    concepto = concepto.ToUpper();

    if (concepto.Contains("TRANSFERENCIA"))
        return "🔄";
    if (concepto.Contains("PAGO") || concepto.Contains("TARJETA"))
        return "💳";
    if (concepto.Contains("RECIBO") || concepto.Contains("REMESA") || concepto.Contains("DOMICILIACION"))
        return "🧾";
    if (concepto.Contains("NOMINA") || concepto.Contains("SUELDO"))
        return "🧑‍💼";
    if (concepto.Contains("RETIRO") || concepto.Contains("CAJERO") || concepto.Contains("EFECTIVO"))
        return "💵";
    if (concepto.Contains("VACACIONES") || concepto.Contains("VIAJE"))
        return "✈️";
    if (concepto.Contains("SEGURO"))
        return "🛡️";
    if (concepto.Contains("LIMPIEZA"))
        return "🧹";
    if (concepto.Contains("REPARACION") || concepto.Contains("REPARACIONES") || concepto.Contains("MANTENIMIENTO"))
        return "🛠️";
    if (concepto.Contains("MAPFRE") || concepto.Contains("ASEGURADORA"))
        return "🏥";
    if (concepto.Contains("OTROS") || concepto.Contains("DESCONOCIDO"))
        return "❔";

    // Por defecto
    return "❔";
}

    }
}