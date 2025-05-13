using System.Globalization;
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
        public async Task<ResponseBase<FinancialDashboardData>> GetBankMovement(FilterRequest request) 
        {
            string usr = "ffl1.president@vtortosa.nom.es";
            string password = "FFL.1234";
            var token = await _authenticationService.GetTokenAsync(usr,password);
            var bankDto = await _searchService.GetBankMovementAsync(token, request);
            

            var mappedBanks = _mapper.Map<List<BankMovement>>(bankDto.Data);

            var orderedMovements = mappedBanks.OrderByDescending(m => m.MovementId).ToList();
            decimal currentTotal = orderedMovements.First().Total;

            var calculatedMovements = await ProcesarMovimientos(mappedBanks);

            int currentYear = DateTime.Now.Year;

            var categorias = new Dictionary<string, string>
            {
                { "DOMICILIACION", "Domiciliación" },
                { "ADEUDO", "Adeudos" },
                { "MANTENIMIENTO", "Mantenimiento" },
                { "TRANSFERENCIA", "Transferencias" },
                { "PAGO", "Pago" }
            };

            var currentYearMovements = calculatedMovements
                .Where(m => DateTime.Parse(m.Date.ToString()).Year == currentYear)
                .ToList();

            var annualExpenses = calculatedMovements
            .GroupBy(m => DateTime.Parse(m.Date.ToString()).Year)
            .Select(g => new AnnualExpense
            {
                Year = g.Key,
                Value = g.Sum(m => Math.Abs(m.Amount))
            })
            .OrderByDescending(x => x.Year)
            .ToList();

            var totalIngresos = currentYearMovements.Where(m => m.Amount > 0).Sum(m => m.Amount);
            var totalEgresos = currentYearMovements.Where(m => m.Amount < 0).Sum(m => m.Amount);
            var saldo = totalIngresos + totalEgresos;

            var summaryData = new List<SummaryItem>
            {
                new SummaryItem { Title = "Ingreso", Value = totalIngresos.ToString("C2") },
                new SummaryItem { Title = "Egreso", Value = totalEgresos.ToString("C2") },
                new SummaryItem { Title = "Saldo", Value = saldo.ToString("C2") }
            };

            var chartData = currentYearMovements
                .GroupBy(m => DateTime.Parse(m.Date.ToString()).ToString("MMMM", new System.Globalization.CultureInfo("es-ES")))
                .Select(g => new ChartItem
                {
                    Name = g.Key,
                    Ingresos = g.Where(m => m.Amount > 0).Sum(m => m.Amount),
                    Egresos = g.Where(m => m.Amount < 0).Sum(m => Math.Abs(m.Amount))
                })
                .OrderBy(c => DateTime.ParseExact(c.Name, "MMMM", new System.Globalization.CultureInfo("es-ES")))
                .ToList();

            var servicePayments = calculatedMovements
                .Where(m => m.Amount < 0 && DateTime.Parse(m.Date.ToString()).Year == currentYear)
                .Select(m => new 
                {
                    Value = Math.Abs(m.Amount),
                    Name = categorias.FirstOrDefault(c => 
                        m.Concept != null && m.Concept.ToUpper().Contains(c.Key)
                    ).Value ?? "Otros"
                })
                .GroupBy(m => m.Name)
                .Select(g => new ServicePayment
                {
                    Name = g.Key,
                    Value = g.Sum(x => x.Value)
                })
                .ToList();

            var totalsData = new List<TotalData>
            {
                new TotalData { Name = "Total Ingresos", Value = totalIngresos },
                new TotalData { Name = "Total Egresos", Value = Math.Abs(totalEgresos) }
            };

            var financialDashboardData = new FinancialDashboardData
                {
                    AnnualExpenses = annualExpenses,
                    SummaryData = summaryData,
                    ChartData = chartData,
                    ServicePayments = servicePayments,
                    TotalsData = totalsData,
                    BankMovements = calculatedMovements.Where(x=> DateTime.Parse(x.Date.ToString()).Year == currentYear) .ToList()
                };

            var response = new ResponseBase<FinancialDashboardData>()
            {
                Code = bankDto.Code,
                Message = bankDto.Message,
                Data = financialDashboardData
            };
            return response;   
        }
        public async Task<string> ObtenerIcono(string concepto)
        {
            string apiKey = "sk-1YIxEBduO7Ycdu4f7gUG02sADh3Tzbu4N6NNTYyMq3T3BlbkFJKSBJn53XSJ8jhggwXvOE-93GBCRx4ufKocHDx-VCcA";

            var icono = await OpenAIService.ObtenerIconoConIA(concepto, apiKey);
            var texto = new StringInfo(icono);
            return texto.SubstringByTextElements(0, 1);
        }
        public async Task<List<BankMovement>> ProcesarMovimientos(List<BankMovement> mappedBanks)
        {
            var calculatedMovements = new List<BankMovement>();
            decimal currentTotal = 0;

            var iconTasks = mappedBanks
                .OrderBy(m => m.MovementId)
                .Select(async m =>
                {
                    var icon = m.Concept == "" ? "" : await ObtenerIcono(m.Concept);
                    return new { Movement = m, Icon = icon };
                })
                .ToList();

            var iconResults = await Task.WhenAll(iconTasks);

            foreach (var result in iconResults)
            {
                var m = result.Movement;
                currentTotal += m.Amount;

                var bankMovement = new BankMovement
                {
                    MovementId = m.MovementId,
                    Date = m.Date,
                    Concept = m.Concept,
                    Amount = m.Amount,
                    Total = currentTotal,
                    Icon = result.Icon
                };

                calculatedMovements.Add(bankMovement);
            }

            return calculatedMovements;
        }

    }
}