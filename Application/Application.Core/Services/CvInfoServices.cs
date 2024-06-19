using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using Framework.Core.Helpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO.Compression;

namespace Application.Core.Services.Core
{
    public class CvInfoServices : BaseService, ICvInfoServices
    {
        private readonly IRepository<CvInfo> cvInfoRepository;
        private int _minNumberOfProject = 6;
        private string _templatePath = "..\\..\\Presentation\\WebAPI\\Assets\\CvInfo";

        public CvInfoServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            cvInfoRepository = _unitOfWork.GetRepository<CvInfo>();
        }

        public async Task<PagedList<CvInfoResponse>> GetPaged(RequestPaged request)
        {
            string[] sortParts = request.sort.Split('.');
            var queryField = sortParts[0];
            var order = sortParts[1];

            // For sorting by technical
            Guid requestGuid;
            bool isGuid = Guid.TryParse(queryField, out requestGuid);

            PagedList<CvInfo> cvInfos;
            var query = cvInfoRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => string.IsNullOrEmpty(request.search) || x.name.ToLower().Contains(request.search.ToLower()))
                        .Include(x => x.cvTechInfos)
                        .Include(y => y.bizInfos);
            if (isGuid)
            {
                if (order == "asc")
                {
                    cvInfos = await query
                            .OrderBy(x => x.cvTechInfos.FirstOrDefault(y => y.TechnicalId == requestGuid).Value) // Sort by Value of the specified TechnicalId
                            .ThenBy(x => x.user_code)
                            .ToPagedListAsync(request.page, request.size);
                }
                else
                {
                    cvInfos = await query
                            .OrderBy(x => x.cvTechInfos.FirstOrDefault(y => y.TechnicalId == requestGuid).Value == null ? 1 : 0)
                            .ThenByDescending(x => x.cvTechInfos.FirstOrDefault(y => y.TechnicalId == requestGuid).Value) // Sort by Value of the specified TechnicalId
                            .ThenByDescending(x => x.user_code)
                            .ToPagedListAsync(request.page, request.size);
                }
            }
            else
            {
                cvInfos = await cvInfoRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => string.IsNullOrEmpty(request.search) || x.name.ToLower().Contains(request.search.ToLower()))
                        .SortBy(request.sort ?? "user_code.asc")
                        .Include(x => x.cvTechInfos)
                        .Include(y => y.bizInfos)
                        .ToPagedListAsync(request.page, request.size);
            }

            foreach (var cvInfo in cvInfos.data)
            {
                cvInfo.cvTechInfos = cvInfo?.cvTechInfos?.Where(cvTechInfo => !cvTechInfo.del_flg).ToList();
                cvInfo.bizInfos = cvInfo?.bizInfos?.Where(bizInfo => !bizInfo.del_flg).ToList();
            }

            var dataMapping = _mapper.Map<PagedList<CvInfoResponse>>(cvInfos);

            return dataMapping;
        }

        public async Task<IList<CvInfoResponse>> GetList()
        {
            var data = await cvInfoRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<CvInfoResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<CvInfo>, List<CvInfoResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<CvInfoResponse> GetById(Guid id)
        {
            var entity = cvInfoRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .Include(x => x.cvTechInfos)
                                    .ThenInclude(x => x.Technical)
                                  .Include(x => x.bizInfos)
                                  .FirstOrDefault();
            if (entity != null)
            {
                entity.cvTechInfos = entity.cvTechInfos
                                           .Where(x => !x.del_flg)
                                           .ToList();
                entity.bizInfos = entity.bizInfos
                                           .Where(x => !x.del_flg)
                                           .ToList();
            }

            var data = _mapper.Map<CvInfoResponse>(entity);

            return data;
        }

        public async Task<int> Create(CvInfoRequest request)
        {
            var count = 0;

            request = ValidateRequest(request);

            var cvInfo = _mapper.Map<CvInfo>(request);

            await cvInfoRepository.AddEntityAsync(cvInfo);

            count += await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, CvInfoRequest request)
        {
            var count = 0;

            var entity = _unitOfWork
                            .GetRepository<CvInfo>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);

            await cvInfoRepository.UpdateEntityAsync(entity);

            var cvTechEntityList = _unitOfWork.GetRepository<CvTechnicalInfo>()
                                            .GetQuery()
                                            .ExcludeSoftDeleted()
                                            .Where(x => x.CvInfoId == id);

            var bizInfoEntityList = _unitOfWork.GetRepository<BizInfo>()
                                                .GetQuery()
                                                .ExcludeSoftDeleted()
                                                .Where(x => x.cvInfoId == id);

            var cvTechRequestList = request.cvTechInfos;

            var bizInfoRequestList = request.bizInfos;

            if (cvTechRequestList?.Count > 0)
            {
                // Delete cvtechnical
                foreach (var itemcvTechEntity in cvTechEntityList)
                {
                    var cvTechinRq = cvTechRequestList.Find(x => x.id == itemcvTechEntity.id);

                    if (cvTechinRq == null)
                    {
                        await _unitOfWork.GetRepository<CvTechnicalInfo>()
                                    .DeleteEntityAsync(itemcvTechEntity);
                    }
                }
            }
            else
            {
                foreach (var itemcvTechEntity in cvTechEntityList)
                {
                    await _unitOfWork.GetRepository<CvTechnicalInfo>()
                                    .DeleteEntityAsync(itemcvTechEntity);
                }
            }

            if (bizInfoRequestList?.Count > 0)
            {
                // Update or Delete biz info
                foreach (var itemBizEntity in bizInfoEntityList)
                {
                    var cvBizinRq = bizInfoRequestList.Find(x => x.id == itemBizEntity.id);

                    if (cvBizinRq == null)
                    {
                        await _unitOfWork.GetRepository<BizInfo>()
                                    .DeleteEntityAsync(itemBizEntity);
                    }
                }
            }
            else
            {
                foreach (var itemBizEntity in bizInfoEntityList)
                {
                    await _unitOfWork.GetRepository<BizInfo>()
                                    .DeleteEntityAsync(itemBizEntity);
                }
            }

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = cvInfoRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await cvInfoRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<byte[]> ExportAndZipAllCVs()
        {
            IList<CvInfoResponse> cvInfos = await GetList();
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (CvInfoResponse? cv in cvInfos)
                    {
                        var cvData = await ExportAndZipCVDetail(cv.id);
                        if (cvData != null)
                        {
                            var entry = archive.CreateEntry($"CV_{cv.name}_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.zip");
                            using (var entryStream = entry.Open())
                            {
                                entryStream.Write(cvData, 0, cvData.Length);
                            }
                        }
                    }
                }
                return ms.ToArray();
            }
        }

        public async Task<byte[]> ExportAndZipCVDetail(Guid id)
        {
            List<string> langList = new List<string>() { "en", "jp" };
            CvInfoResponse cvInfo = await GetById(id);
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (string lang in langList)
                    {
                        var cvData = await ExportExcelCVDetail(cvInfo, lang);
                        if (cvData != null)
                        {
                            var entry = archive.CreateEntry($"CV_{cvInfo.name}_{lang.ToUpperInvariant()}_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.xlsx");
                            using (var entryStream = entry.Open())
                            {
                                entryStream.Write(cvData, 0, cvData.Length);
                            }
                        }
                    }
                }
                return ms.ToArray();
            }
        }

        public async Task<byte[]> ExportAndZipCVTemplate()
        {
            List<string> langList = new List<string>() { "en", "jp" };
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (string lang in langList)
                    {
                        var cvData = await ExportExcelCVTemplate(lang);
                        if (cvData != null)
                        {
                            var entry = archive.CreateEntry($"CV_template_{lang.ToUpperInvariant()}.xlsx");
                            using (var entryStream = entry.Open())
                            {
                                entryStream.Write(cvData, 0, cvData.Length);
                            }
                        }
                    }
                }
                return ms.ToArray();
            }
        }

        #region private
        private async Task<byte[]> ExportExcelCVTemplate(string lang)
        {
            string selectedLang = "EN";
            if (lang == "jp") { selectedLang = "JP"; }

            using (var ms = new MemoryStream())
            {
                await AddFileToMemoryStream(ms, $"{_templatePath}\\CV_{selectedLang}.xlsx");

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(ms, true))
                {
                    WorkbookPart workbookPart = document.WorkbookPart;
                    workbookPart.InsertCell("BI1", DateTime.UtcNow.Date.ToString("dd/MM/yyyy"), DataType.TEXT, VCellStyle.CVDefault);
                    document.WorkbookPart.Workbook.Save();
                }

                return ms.ToArray();
            }

        }

        private async Task<byte[]> ExportExcelCVDetail(CvInfoResponse cvInfo, string lang)
        {
            string selectedLang = "EN";
            int selectedLangIndex = 0;
            if (lang == "jp") { selectedLang = "JP"; selectedLangIndex = 1; }

            using (var ms = new MemoryStream())
            {
                await AddFileToMemoryStream(ms, $"{_templatePath}\\CV_{selectedLang}.xlsx");

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(ms, true))
                {
                    WorkbookPart workbookPart = document.WorkbookPart;
                    workbookPart.InsertCell("BI1", DateTime.UtcNow.Date.ToString("dd/MM/yyyy"), DataType.TEXT, VCellStyle.CVDefault);
                    InsertData(workbookPart, cvInfo, selectedLangIndex);
                    document.WorkbookPart.Workbook.Save();
                }

                return ms.ToArray();
            }

        }

        private async Task AddFileToMemoryStream(MemoryStream ms, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fileStream.CopyTo(ms);
                ms.Position = 0;
            }
        }


        private CvInfoRequest ValidateRequest(CvInfoRequest request)
        {
            if (request.lang1_hearing < 1 || request.lang1_hearing > 3)
            {
                request.lang1_hearing = null;
            }
            if (request.lang1_speaking < 1 || request.lang1_speaking > 3)
            {
                request.lang1_speaking = null;
            }
            if (request.lang1_reading < 1 || request.lang1_reading > 3)
            {
                request.lang1_reading = null;
            }
            if (request.lang1_writing < 1 || request.lang1_writing > 3)
            {
                request.lang1_writing = null;
            }
            if (request.lang2_hearing < 1 || request.lang2_hearing > 3)
            {
                request.lang2_hearing = null;
            }
            if (request.lang2_speaking < 1 || request.lang2_speaking > 3)
            {
                request.lang2_speaking = null;
            }
            if (request.lang2_reading < 1 || request.lang2_reading > 3)
            {
                request.lang2_reading = null;
            }
            if (request.lang2_writing < 1 || request.lang2_writing > 3)
            {
                request.lang2_writing = null;
            }

            return request;
        }

        private void InsertData(WorkbookPart workbookPart, CvInfoResponse cvInfo, int selectedLang)
        {
            List<string> langRatings = new List<string> { "", "○", "☆", "△" };
            List<List<string>> genders = new List<List<string>> { new List<string> { "Unknown", "Male", "Female" }, new List<string> { "不明", "男", "女" } };
            Border thinBorder = new Border(
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
                    );

            // ############# TOP SECTION ##############
            workbookPart.InsertCell("J2", cvInfo.furigana ?? "", DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("J4", cvInfo.name.ToUpperInvariant().RemoveAccents().Replace("Đ", "D").Replace("đ", "d") ?? "", DataType.TEXT, VCellStyle.CVDefault);

            if (cvInfo?.gender != null)
            {
                workbookPart.InsertCell("AF4", genders[selectedLang][cvInfo.gender] ?? "", DataType.TEXT, VCellStyle.CVDefault);
            }
            if (cvInfo?.birthday != null)
            {
                int birthdayYear = cvInfo.birthday.Year;
                int currentYear = DateTime.Now.Year;
                int age = currentYear - birthdayYear;
                workbookPart.InsertCell("AJ4", age.ToString(), DataType.NUMBER, VCellStyle.CVDefault);
            }

            workbookPart.InsertCell("AT5", langRatings[cvInfo.lang1_hearing ?? 0], DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AW5", langRatings[cvInfo.lang1_speaking ?? 0], DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("BA5", langRatings[cvInfo.lang1_reading ?? 0], DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("BE5", langRatings[cvInfo.lang1_writing ?? 0], DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AT6", langRatings[cvInfo.lang2_hearing ?? 0], DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AW6", langRatings[cvInfo.lang2_speaking ?? 0], DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("BA6", langRatings[cvInfo.lang2_reading ?? 0], DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("BE6", langRatings[cvInfo.lang2_writing ?? 0], DataType.TEXT, VCellStyle.CVDefault);

            workbookPart.InsertCell("J13", selectedLang == 0 ? cvInfo.work_process : cvInfo.work_process_jp, DataType.TEXT, VCellStyle.CVDefaultWrap);
            workbookPart.InsertCell("J8", selectedLang == 0 ? cvInfo.last_university_name : cvInfo.last_university_name_jp, DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AD8", selectedLang == 0 ? cvInfo.subject : cvInfo.subject_jp, DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AP8", cvInfo.graduation_year.ToString() ?? "", DataType.NUMBER, VCellStyle.CVDefault);
            workbookPart.InsertCell("J22", selectedLang == 0 ? cvInfo.note : cvInfo.note_jp, DataType.TEXT, VCellStyle.CVDefaultWrap);

            workbookPart.InsertCell("K10", selectedLang == 0 ? cvInfo.certificate1_name : cvInfo.certificate1_name_jp, DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AW10", cvInfo.certificate1_year.ToString() ?? "", DataType.NUMBER, VCellStyle.CVDefault);
            workbookPart.InsertCell("K11", selectedLang == 0 ? cvInfo.certificate2_name : cvInfo.certificate2_name_jp, DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AW11", cvInfo.certificate2_year.ToString() ?? "", DataType.NUMBER, VCellStyle.CVDefault);
            workbookPart.InsertCell("K12", selectedLang == 0 ? cvInfo.certificate3_name : cvInfo.certificate3_name_jp, DataType.TEXT, VCellStyle.CVDefault);
            workbookPart.InsertCell("AW12", cvInfo.certificate3_year.ToString() ?? "", DataType.NUMBER, VCellStyle.CVDefault);

            // ############# MID SECTION ##############

            var OSList = new List<string>{
                        "Win 7",
                        "Other win",
                        "iOS",
                        "Android",
                        "Linux",
                    };

            for (int i = 0; i < OSList.Count; i++)
            {
                var OS = OSList[i];
                var years = cvInfo.cvTechInfos.FirstOrDefault(x => x.Technical.Name == OS)?.value ?? 0;
                if (years > 0)
                    workbookPart.InsertCell($"J{i + 5 + 27}", years.ToString(), DataType.NUMBER, VCellStyle.CVDefault);
            }

            var FrameWorkList = new List<string>{
                        ".Net",
                        "MVC",
                        "Symfony",
                        "Drupal",
                        "ReactJS",
                        "VueJs",
                        "Node.js",
                        "Bootstrap",
                    };
            for (int i = 0; i < FrameWorkList.Count; i++)
            {
                var FrameWork = FrameWorkList[i];
                var years = cvInfo.cvTechInfos.FirstOrDefault(x => x.Technical.Name == FrameWork)?.value ?? 0;
                if (years > 0)
                    workbookPart.InsertCell($"J{i + 16 + 27}", years.ToString(), DataType.NUMBER, VCellStyle.CVDefault);
            }

            var LanguageList = new List<string>{
                        "Assembly",
                        "COBOL",
                        "PL/SQL",
                        "PRO*C",
                        "C",
                        "C++",
                        "VB6",
                        "Power Builder",
                        "PHP",
                        "ABAP",
                        "C#.net",
                        "VB.NET",
                        "Java",
                        "Perl",
                        "Shell script",
                        "Delphi",
                        "Python",

                    };

            for (int i = 0; i < FrameWorkList.Count; i++)
            {
                var FrameWork = FrameWorkList[i];
                var years = cvInfo.cvTechInfos.FirstOrDefault(x => x.Technical.Name == FrameWork)?.value ?? 0;
                if (years > 0)
                    workbookPart.InsertCell($"W{i + 5 + 27}", years.ToString(), DataType.NUMBER, VCellStyle.CVDefault);
            }


            var DBList = new List<string>{
                        "Oracle",
                        "SQL Server",
                        "PostgreSQL",
                        "MySQL",
                        "Sybase",
                        "Informix",
                        "ISAM",
                        "DB2",
                        "Access",
                        "DynamoDB",
                        "IMS",
                        "MongoDB",
                    };


            for (int i = 0; i < DBList.Count; i++)
            {
                var DB = DBList[i];
                var years = cvInfo.cvTechInfos.FirstOrDefault(x => x.Technical.Name == DB)?.value ?? 0;
                if (years > 0)
                    workbookPart.InsertCell($"AJ{i + 5 + 27}", years.ToString(), DataType.NUMBER, VCellStyle.CVDefault);
            }

            var WebTeckList = new List<string>{
                        "HTML",
                        "XML",
                        "Java Script",
                        "ASP",
                        "JSP",
                        "NGINX",
                        "XAMPP",
                        "IIS",
                        "Tomcat",
                        "Jboss",
                        "Oracle AS",
                        "WebLogic",
                        "WebSpher",
                        "Coldfusion",
                    };

            for (int i = 0; i < WebTeckList.Count; i++)
            {
                var WebTeck = WebTeckList[i];
                var years = cvInfo?.cvTechInfos?.FirstOrDefault(x => x?.Technical?.Name == WebTeck)?.value ?? 0;
                if (years > 0)
                    workbookPart.InsertCell($"AW{i + 5 + 27}", years.ToString(), DataType.NUMBER, VCellStyle.CVDefault);
            }
            var ProcessList = new List<string>{
                        "System proposal",
                        "System analysis",
                        "Overview design",
                        "Basic design",
                        "Function design",
                        "Detail design",
                        "Coding design",
                        "Coding",
                        "Unit Test",
                        "IT Test",
                        "Operation guidance",
                        "Maintenance",
                        "Operation",
                        "Project management",
                        "Estimate",
                        "Customer education",
                        "Manual creation",

                    };

            for (int i = 0; i < ProcessList.Count; i++)
            {
                var Process = ProcessList[i];
                var years = cvInfo.cvTechInfos.FirstOrDefault(x => x.Technical.Name == Process)?.value ?? 0;
                if (years > 0)
                    workbookPart.InsertCell($"BJ{i + 5 + 27}", years.ToString(), DataType.NUMBER, VCellStyle.CVDefault);

            }

            // ############# BOTTOM SECTION ##############
            int startRowData = 57;
            int projectCount = cvInfo.bizInfos.Count();

            // Draw the missing border
            if (projectCount > _minNumberOfProject)
            {
                int missBorderProject = projectCount - _minNumberOfProject;
                int startBlock = 57 + 5 * _minNumberOfProject;
                int endBlock = startBlock + 4;

                workbookPart.SetBorderAll($"B{startBlock}:BO{endBlock + (missBorderProject - 1) * 5}", BorderStyleValues.Thin);

                for (int i = 0; i < missBorderProject; i++)
                {
                    workbookPart.MergeCell($"B{startBlock}:B{endBlock}");
                    workbookPart.MergeCell($"C{startBlock}:N{endBlock}");
                    workbookPart.InsertCell($"B{startBlock}", $"{_minNumberOfProject + i + 1}", DataType.NUMBER, VCellStyle.CVDefaultBorderSmall);
                    workbookPart.SetBorderAll($"AA{startBlock}:AD{endBlock}", BorderStyleValues.None);
                    workbookPart.SetRangeBorder($"AA{startBlock}:AD{endBlock}", thinBorder);

                    for (int j = startBlock; j <= endBlock; j++)
                    {
                        workbookPart.MergeCell($"AA{j}:AD{j}");
                    }

                    workbookPart.MergeCell($"O{startBlock}:Z{endBlock}");
                    workbookPart.MergeCell($"AE{startBlock}:AF{endBlock}");
                    workbookPart.MergeCell($"AG{startBlock}:AH{endBlock}");
                    workbookPart.MergeCell($"AI{startBlock}:AJ{endBlock}");
                    workbookPart.MergeCell($"AK{startBlock}:AL{endBlock}");
                    workbookPart.MergeCell($"AM{startBlock}:AN{endBlock}");
                    workbookPart.MergeCell($"AO{startBlock}:AP{endBlock}");
                    workbookPart.MergeCell($"AQ{startBlock}:AR{endBlock}");
                    workbookPart.MergeCell($"AS{startBlock}:AT{endBlock}");
                    workbookPart.MergeCell($"AU{startBlock}:AZ{endBlock}");
                    workbookPart.MergeCell($"BA{startBlock}:BG{endBlock}");
                    workbookPart.MergeCell($"BH{startBlock}:BO{endBlock}");

                    startBlock = endBlock + 1;
                    endBlock = startBlock + 4;
                }
            }

            // Insert data
            for (int i = 0; i < projectCount; i++)
            {
                var project = cvInfo.bizInfos[i];

                workbookPart.InsertCell($"C{startRowData}", selectedLang == 0 ? project.prj_name : project.prj_name_jp, DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Project
                workbookPart.InsertCell($"O{startRowData}", selectedLang == 0 ? project.prj_content : project.prj_content_jp, DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Duties / Comments
                workbookPart.InsertCell($"AA{startRowData + 2}", $"{project.period}M", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Period
                workbookPart.InsertCell($"AE{startRowData}", project.system_analysis ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // System analysis
                workbookPart.InsertCell($"AG{startRowData}", project.overview_design ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Overview design
                workbookPart.InsertCell($"AI{startRowData}", project.basic_design ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Basic design
                workbookPart.InsertCell($"AK{startRowData}", project.functional_design ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Function design
                workbookPart.InsertCell($"AM{startRowData}", project.detailed_design ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Detail design
                workbookPart.InsertCell($"AO{startRowData}", project.coding ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Coding
                workbookPart.InsertCell($"AQ{startRowData}", project.unit_test ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Unit Test
                workbookPart.InsertCell($"AS{startRowData}", project.operation ? "○" : "", DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Operation
                workbookPart.InsertCell($"AU{startRowData}", project.os_db, DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // OS・DB
                workbookPart.InsertCell($"BA{startRowData}", project.language, DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Language
                workbookPart.InsertCell($"BH{startRowData}", selectedLang == 0 ? project.role : project.role_jp, DataType.TEXT, VCellStyle.CVDefaultWrapSmall); // Role

                if (i >= _minNumberOfProject)
                {
                    workbookPart.CopyCellStyleRange($"B{startRowData - 5}:BO{startRowData - 5}", $"B{startRowData}");
                    workbookPart.CopyCellStyle($"AA{startRowData - 5 + 2}", $"AA{startRowData + 2}");
                }
                startRowData += 5;
            }

        }

        #endregion
    }



}
