using Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand;
using Application.Modules.FacultiesModule.Commands.FacultyAddCommand;
using Application.Modules.GroupsModule.Commands.GroupAddCommand;
using Application.Modules.LessonSchedulesModule.Commands.LessonScheduleAddCommand;
using Application.Modules.LessonsModule.Commands.LessonAddCommand;
using Application.Modules.RoomsModule.Commands.RoomAddCommand;
using Application.Modules.StudentsModule.Commands.StudentRegisterCommand;
using Application.Modules.SubjectsModule;
using Application.Modules.SubjectsModule.Commands.SubjectAddCommand;
using Application.Modules.TeachersModule.Commands.TeacherRegisterCommand;
using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Domain.Models.Stables;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Presentation.AppCode.Seeds
{
    public class DataSeeder
    {
        private readonly ILogger<DataSeeder> logger;
        private readonly IMediator mediator;
        private readonly IFacultyRepository facultyRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IGroupRepository groupRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly IRoomRepository roomRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly ISubjectTopicRepository subjectTopicRepository;
        private readonly ISubjectMaterialRepository subjectMaterialRepository;
        private readonly ISubjectLiteratureRepository subjectLiteratureRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly ILessonScheduleRepository lessonScheduleRepository;
        private readonly DataContext db;
        private readonly IAttendanceRepository attendanceRepository;

        public DataSeeder(
            ILogger<DataSeeder> logger,
            IMediator mediator,
            IFacultyRepository facultyRepository,
            IStudentRepository studentRepository,
            IDepartmentRepository departmentRepository,
            IGroupRepository groupRepository,
            ITeacherRepository teacherRepository,
            IRoomRepository roomRepository,
            ISubjectRepository subjectRepository,
            ISubjectTopicRepository subjectTopicRepository,
            ISubjectMaterialRepository subjectMaterialRepository,
            ISubjectLiteratureRepository subjectLiteratureRepository,
            ILessonRepository lessonRepository,
            ILessonScheduleRepository lessonScheduleRepository,
            DataContext db,
            IAttendanceRepository attendanceRepository)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.facultyRepository = facultyRepository;
            this.studentRepository = studentRepository;
            this.departmentRepository = departmentRepository;
            this.groupRepository = groupRepository;
            this.teacherRepository = teacherRepository;
            this.roomRepository = roomRepository;
            this.subjectRepository = subjectRepository;
            this.subjectTopicRepository = subjectTopicRepository;
            this.subjectMaterialRepository = subjectMaterialRepository;
            this.subjectLiteratureRepository = subjectLiteratureRepository;
            this.lessonRepository = lessonRepository;
            this.lessonScheduleRepository = lessonScheduleRepository;
            this.db = db;
            this.attendanceRepository = attendanceRepository;
        }

        public async Task SeedAsync()
        {
            await RunSeederStepAsync(nameof(SeedFacultiesAsync), SeedFacultiesAsync);
            await RunSeederStepAsync(nameof(SeedDepartmentsAsync), SeedDepartmentsAsync);
            await RunSeederStepAsync(nameof(SeedTeachersAsync), SeedTeachersAsync);
            await RunSeederStepAsync(nameof(SeedStudentsAsync), SeedStudentsAsync);
            await RunSeederStepAsync(nameof(SeedGroupsAsync), SeedGroupsAsync);
            await RunSeederStepAsync(nameof(SeedRoomsAsync), SeedRoomsAsync);
            await RunSeederStepAsync(nameof(SeedSubjectsAsync), SeedSubjectsAsync);
            await RunSeederStepAsync(nameof(SeedSubjectContentsAsync), SeedSubjectContentsAsync);
            await RunSeederStepAsync(nameof(SeedLessonsAsync), SeedLessonsAsync);
            await RunSeederStepAsync(nameof(SeedLessonSchedulesAsync), SeedLessonSchedulesAsync);
            await RunSeederStepAsync(nameof(SeedLessonGroupsAsync), SeedLessonGroupsAsync);
            await RunSeederStepAsync(nameof(SeedSubjectCatalogMetadataAsync), SeedSubjectCatalogMetadataAsync);
            await RunSeederStepAsync(nameof(SeedAdditionalLessonSchedulesAsync), SeedAdditionalLessonSchedulesAsync);
            await RunSeederStepAsync(nameof(SeedAttendanceSessionsAsync), SeedAttendanceSessionsAsync);
        }

        private async Task RunSeederStepAsync(string stepName, Func<Task> step)
        {
            try
            {
                await step().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Data seeder step {StepName} failed; later steps will still run.", stepName);
            }
        }

        /// <summary>Duplicate key / unique index violations (including filtered unique indexes after soft-delete).</summary>
        private static bool IsSqlUniqueOrDuplicateKey(Exception ex)
        {
            for (var e = ex; e != null; e = e.InnerException)
            {
                if (e is SqlException sql && (sql.Number == 2601 || sql.Number == 2627))
                    return true;
            }

            return false;
        }

        private static bool IsSqlInvalidColumn(Exception ex)
        {
            for (var e = ex; e != null; e = e.InnerException)
            {
                if (e is SqlException sql && sql.Number == 207)
                    return true;
            }

            return false;
        }

        private async Task SeedFacultiesAsync()
        {
            var faculties = new List<string>
            {
                "Transport və Logistika",
                "Energetika",
                "Maşınqayırma və Metallurgiya",
                "İnformasiya Texnologiyaları və Telekommunikasiya",
                "Xüsusi Texnika və Texnologiya",
                "Sənaye İqtisadiyyatı və İdarəetmə"
            };

            foreach (var name in faculties)
            {
                // Idempotent by name: do not skip entire seed when DB already has other faculty rows.
                if (facultyRepository.GetAll().Any(f => f.Name == name))
                    continue;

                try
                {
                    await mediator.Send(new FacultyAddRequest { Name = name }).ConfigureAwait(false);
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Faculty seed skipped (already exists): {Name}", name);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Faculty seed failed for {Name}", name);
                }
            }
        }

        private async Task SeedDepartmentsAsync()
        {
            if (departmentRepository.GetAll().Any())
                return;

            // Get seeded faculty ids in order
            var facultyList = facultyRepository.GetAll()
                .OrderBy(f => f.Id)
                .ToList();

            const string transportName = "Transport və Logistika";
            const string energyName = "Energetika";
            const string mechanicalName = "Maşınqayırma və Metallurgiya";
            const string itName = "İnformasiya Texnologiyaları və Telekommunikasiya";
            const string specialName = "Xüsusi Texnika və Texnologiya";
            const string economicsName = "Sənaye İqtisadiyyatı və İdarəetmə";

            var requiredNames = new[]
            {
                transportName, energyName, mechanicalName, itName, specialName, economicsName
            };

            foreach (var required in requiredNames)
            {
                if (facultyList.All(f => f.Name != required))
                {
                    logger.LogWarning(
                        "Department seed skipped: faculty {FacultyName} not found. Ensure faculty seed ran successfully.",
                        required);
                    return;
                }
            }

            // Map by name to be safe
            var transport = facultyList.First(f => f.Name == transportName).Id;
            var energy = facultyList.First(f => f.Name == energyName).Id;
            var mechanical = facultyList.First(f => f.Name == mechanicalName).Id;
            var it = facultyList.First(f => f.Name == itName).Id;
            var special = facultyList.First(f => f.Name == specialName).Id;
            var economics = facultyList.First(f => f.Name == economicsName).Id;

            var departments = new List<DepartmentAddRequest>
            {
                // Transport və Logistika
                new() { Name = "Nəqliyyat texnikası və idarəetmə texnologiyaları", FacultyId = transport },
                new() { Name = "Nəqliyyat logistikası və yol hərəkətinin təhlükəsizliyi", FacultyId = transport },

                // Energetika
                new() { Name = "Elektrotexnika", FacultyId = energy },
                new() { Name = "Mühəndis fizikası və elektronika", FacultyId = energy },
                new() { Name = "Enerji səmərəliliyi və yaşıl enerji texnologiyaları", FacultyId = energy },

                // Maşınqayırma və Metallurgiya
                new() { Name = "Maşın konstruksiyası, mexatronika və sənaye texnologiyaları", FacultyId = mechanical },
                new() { Name = "Maşınqayırma texnologiyası", FacultyId = mechanical },
                new() { Name = "Metallurgiya və materiallar texnologiyası", FacultyId = mechanical },
                new() { Name = "Kimya texnologiyası, emal və ekologiya", FacultyId = mechanical },
                new() { Name = "Mexanika", FacultyId = mechanical },

                // İnformasiya Texnologiyaları və Telekommunikasiya
                new() { Name = "Mühəndis riyaziyyatı və süni intellekt", FacultyId = it },
                new() { Name = "Radioelektronika və telekommunikasiya mühəndisliyi", FacultyId = it },
                new() { Name = "Kompüter texnologiyaları", FacultyId = it },
                new() { Name = "Kibertəhlükəsizlik", FacultyId = it },

                // Xüsusi Texnika və Texnologiya
                new() { Name = "Xüsusi texnologiyalar və avadanlıq", FacultyId = special },
                new() { Name = "Müdafiə sistemləri və texnoloji inteqrasiya", FacultyId = special },
                new() { Name = "Humanitar fənlər", FacultyId = special },
                new() { Name = "Xarici dillər", FacultyId = special },

                // Sənaye İqtisadiyyatı və İdarəetmə
                new() { Name = "Sənaye mühəndisliyi və davamlı iqtisadiyyat", FacultyId = economics },
                new() { Name = "Biznes idarəetməsi", FacultyId = economics },
                new() { Name = "Rəqəmsal iqtisadiyyat və maliyyə texnologiyaları", FacultyId = economics },
            };

            foreach (var request in departments)
            {
                try
                {
                    await mediator.Send(request).ConfigureAwait(false);
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Department seed skipped (duplicate): {Name}", request.Name);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Department seed failed: {Name}", request.Name);
                }
            }
        }

        private async Task SeedTeachersAsync()
        {
            // Do not use GetAll().Any() — one manually registered teacher would skip all 21 seed rows.
            // Skip only when this seed FIN already exists (same idea as idempotent student seed per FIN).
            var teachers = new List<TeacherRegisterRequest>
            {
                new() { SerialNumber = "MUE0100001", FinCode = "TM01001" },
                new() { SerialNumber = "MUE0100002", FinCode = "TM01002" },
                new() { SerialNumber = "MUE0100003", FinCode = "TM01003" },
                new() { SerialNumber = "MUE0100004", FinCode = "TM01004" },
                new() { SerialNumber = "MUE0100005", FinCode = "TM01005" },
                new() { SerialNumber = "MUE0100006", FinCode = "TM01006" },
                new() { SerialNumber = "MUE0100007", FinCode = "TM01007" },
                new() { SerialNumber = "MUE0100008", FinCode = "TM01008" },
                new() { SerialNumber = "MUE0100009", FinCode = "TM01009" },
                new() { SerialNumber = "MUE0100010", FinCode = "TM01010" },
                new() { SerialNumber = "MUE0100011", FinCode = "TM01011" },
                new() { SerialNumber = "MUE0100012", FinCode = "TM01012" },
                new() { SerialNumber = "MUE0100013", FinCode = "TM01013" },
                new() { SerialNumber = "MUE0100014", FinCode = "TM01014" },
                new() { SerialNumber = "MUE0100015", FinCode = "TM01015" },
                new() { SerialNumber = "MUE0100016", FinCode = "TM01016" },
                new() { SerialNumber = "MUE0100017", FinCode = "TM01017" },
                new() { SerialNumber = "MUE0100018", FinCode = "TM01018" },
                new() { SerialNumber = "MUE0100019", FinCode = "TM01019" },
                new() { SerialNumber = "MUE0100020", FinCode = "TM01020" },
                new() { SerialNumber = "MUE0100021", FinCode = "TM01021" }
            };

            foreach (var request in teachers)
            {
                var fin = request.FinCode.Trim().ToUpperInvariant();
                if (await teacherRepository.GetByFinCodeAsync(fin, CancellationToken.None) is not null)
                    continue;

                try
                {
                    await mediator.Send(request).ConfigureAwait(false);
                }
                catch (ConflictException)
                {
                    // Already registered — skip
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Teacher seed skipped (DB unique constraint): {Fin}", request.FinCode);
                }
                catch (Exception ex) when (IsSqlInvalidColumn(ex))
                {
                    logger.LogError(
                        ex,
                        "Teacher seed failed for {Fin}: database schema is missing columns (run migrations).",
                        request.FinCode);
                    return;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Teacher seed failed for {Fin}", request.FinCode);
                }
            }
        }

        private async Task SeedStudentsAsync()
        {
            if (studentRepository.GetAll().Any())
                return;

            var students = new List<StudentRegisterRequest>
            {
                new() { SerialNumber = "AZE8392014", FinCode = "AB8392K" },
                new() { SerialNumber = "AZE7482931", FinCode = "CD7482M" },
                new() { SerialNumber = "AZE5928374", FinCode = "EF5928P" },
                new() { SerialNumber = "AZE1029384", FinCode = "GH1029R" },
                new() { SerialNumber = "AZE5839201", FinCode = "IJ5839T" },
                new() { SerialNumber = "AZE4728193", FinCode = "KL4728V" },
                new() { SerialNumber = "AZE3617284", FinCode = "MN3617X" },
                new() { SerialNumber = "AZE2506173", FinCode = "OP2506Z" },
                new() { SerialNumber = "AZE1495062", FinCode = "QR1495B" },
                new() { SerialNumber = "AZE9384751", FinCode = "ST9384D" },
                new() { SerialNumber = "AZE8273640", FinCode = "UV8273F" },
                new() { SerialNumber = "AZE7162539", FinCode = "WX7162H" },
                new() { SerialNumber = "AZE6051428", FinCode = "YZ6051J" },
                new() { SerialNumber = "AZE5940317", FinCode = "AA5940L" },
                new() { SerialNumber = "AZE4839206", FinCode = "BB4839N" },
                new() { SerialNumber = "AZE3728195", FinCode = "CC3728Q" },
                new() { SerialNumber = "AZE2617084", FinCode = "DD2617S" },
                new() { SerialNumber = "AZE1506973", FinCode = "EE1506U" },
                new() { SerialNumber = "AZE0495862", FinCode = "FF0495W" },
                new() { SerialNumber = "AZE9384750", FinCode = "GG9384Y" },
                new() { SerialNumber = "AZE8273649", FinCode = "HH8273A" },
                new() { SerialNumber = "AZE7162538", FinCode = "II7162C" },
                new() { SerialNumber = "AZE6051427", FinCode = "JJ6051E" },
                new() { SerialNumber = "AZE5940316", FinCode = "KK5940G" },
                new() { SerialNumber = "AZE4839205", FinCode = "LL4839I" },
                new() { SerialNumber = "AZE3728194", FinCode = "MM3728K" },
                new() { SerialNumber = "AZE2617083", FinCode = "NN2617M" },
                new() { SerialNumber = "AZE1506972", FinCode = "OO1506O" },
                new() { SerialNumber = "AZE0495861", FinCode = "PP0495Q" },
                new() { SerialNumber = "AZE9384759", FinCode = "QQ9384S" },
                new() { SerialNumber = "AZE8273648", FinCode = "RR8273U" },
                new() { SerialNumber = "AZE7162537", FinCode = "SS7162W" },
                new() { SerialNumber = "AZE6051426", FinCode = "TT6051Y" },
                new() { SerialNumber = "AZE5940315", FinCode = "UU5940A" },
                new() { SerialNumber = "AZE4839204", FinCode = "VV4839C" },
                new() { SerialNumber = "AZE3728193", FinCode = "WW3728E" },
                new() { SerialNumber = "AZE2617082", FinCode = "XX2617G" },
                new() { SerialNumber = "AZE1506971", FinCode = "YY1506I" },
                new() { SerialNumber = "AZE0495860", FinCode = "ZZ0495K" },
                new() { SerialNumber = "AZE9384758", FinCode = "AB9384M" },
                new() { SerialNumber = "AZE8273647", FinCode = "CD8273O" },
                new() { SerialNumber = "AZE7162536", FinCode = "EF7162Q" },
                new() { SerialNumber = "AZE6051425", FinCode = "GH6051S" },
                new() { SerialNumber = "AZE5940314", FinCode = "IJ5940U" },
                new() { SerialNumber = "AZE4839203", FinCode = "KL4839W" },
                new() { SerialNumber = "AZE3728192", FinCode = "MN3728Y" },
                new() { SerialNumber = "AZE2617081", FinCode = "OP2617A" },
                new() { SerialNumber = "AZE1506970", FinCode = "QR1506C" },
                new() { SerialNumber = "AZE0495859", FinCode = "ST0495E" },
                new() { SerialNumber = "AZE9384757", FinCode = "UV9384G" }
            };

            foreach (var request in students)
            {
                try
                {
                    await mediator.Send(request).ConfigureAwait(false);
                }
                catch (ConflictException)
                {
                    // Already registered — skip
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Student seed skipped (DB unique constraint): {Fin}", request.FinCode);
                }
                catch (Exception ex) when (IsSqlInvalidColumn(ex))
                {
                    logger.LogError(
                        ex,
                        "Student seed failed for {Fin}: database schema is missing columns (run migrations).",
                        request.FinCode);
                    return;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Student seed failed for {Fin}", request.FinCode);
                }
            }
        }

        private async Task SeedGroupsAsync()
        {
            if (groupRepository.GetAll().Any())
                return;

            var departments = departmentRepository.GetAll()
                .OrderBy(d => d.Id)
                .ToList();

            foreach (var dept in departments)
            {
                var studentIds = studentRepository.GetAll()
                    .Where(s => s.DepartmentId == dept.Id)
                    .Select(s => s.Id)
                    .ToList();

                var request = new GroupAddRequest
                {
                    Name = "1-ci qrup",
                    Year = 1,
                    DepartmentId = dept.Id,
                    StudentIds = studentIds,
                    LessonIds = new List<int>()
                };

                try
                {
                    await mediator.Send(request).ConfigureAwait(false);
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Group seed skipped (duplicate) department {DeptId}", dept.Id);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Group seed failed for department {DeptId} ({DeptName})", dept.Id, dept.Name);
                }
            }
        }

        private async Task SeedRoomsAsync()
        {
            if (roomRepository.GetAll().Any(r => r.BuildingId == 1 && r.Number == 101))
                return;

            var rooms = new List<RoomAddRequest>
            {
                new() { BuildingId = 1, Number = 101 },
                new() { BuildingId = 1, Number = 102 },
                new() { BuildingId = 1, Number = 103 },
                new() { BuildingId = 2, Number = 201 },
                new() { BuildingId = 2, Number = 202 },
                new() { BuildingId = 3, Number = 301 },
                new() { BuildingId = 3, Number = 302 }
            };

            foreach (var request in rooms)
            {
                try
                {
                    await mediator.Send(request).ConfigureAwait(false);
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Room seed skipped (duplicate) {Building}-{Number}", request.BuildingId, request.Number);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Room seed failed {Building}-{Number}", request.BuildingId, request.Number);
                }
            }
        }

        private static string SubjectTitleForDepartment(string departmentName)
        {
            const string prefix = "Ümumi kurikulum — ";
            var max = 200 - prefix.Length;
            if (departmentName.Length <= max)
                return prefix + departmentName;
            return prefix + departmentName[..max];
        }

        private static SubjectAddRequest BuildSeedSubjectAddRequest(int departmentId, string departmentName)
        {
            var title = SubjectTitleForDepartment(departmentName);
            return new SubjectAddRequest
            {
                Name = title,
                DepartmentId = departmentId,
                Term = "2026 Yaz",
                GroupName = $"SEED-D{departmentId}-qrup",
                LectureTeacher = "Seed: Mühazirə müəllimi",
                SeminarTeacher = "Seed: Seminar müəllimi",
                LabTeacher = "yoxdur",
                StudentCount = 30,
                Credits = 5,
                TotalHours = 60,
                WeekCount = 15,
                Purpose = $"Seed fənn üçün qısa məqsəd: {departmentName} üzrə nümunə kurikulum.",
                TeacherMethods = "Mühazirə, seminar, praktiki tapşırıqlar (seed).",
                SyllabusUrl = "https://example.edu/seed/syllabus.pdf",
                FreeWorkScore = 10,
                SeminarScore = 10,
                LabScore = 10,
                AttendanceScore = 10,
                ExamScore = 50,
                Topics = new List<SubjectTopicRowDto>
                {
                    new()
                    {
                        WeekNumber = 1,
                        TopicName = "Giriş və təşkilati məsələlər",
                        TeachingMethods = "Mühazirə, müzakirə",
                        Materials = "Təqdimat, silabus",
                        Equipment = "Proyektor"
                    },
                    new()
                    {
                        WeekNumber = 2,
                        TopicName = "Əsas anlayışlar",
                        TeachingMethods = "Seminar, tapşırıq",
                        Materials = "PDF materiallar",
                        Equipment = "Kompyuter laboratoriyası"
                    },
                    new()
                    {
                        WeekNumber = 3,
                        TopicName = "Tətbiqi nümunələr",
                        TeachingMethods = "Qrup işi",
                        Materials = "Kəsiyyə, nümunə suallar",
                        Equipment = "-"
                    }
                },
                Materials = new List<SubjectMaterialRowDto>
                {
                    new()
                    {
                        Title = "Silabus (seed)",
                        Description = "Avtomatik əlavə olunmuş nümunə silabus.",
                        FileUrl = "https://example.edu/seed/subjects/syllabus.pdf",
                        MaterialType = "Syllabus"
                    },
                    new()
                    {
                        Title = "Mühazirə qeydləri — həftə 1–2",
                        Description = "Seed məzmunu.",
                        FileUrl = "https://example.edu/seed/subjects/lecture-notes.pdf",
                        MaterialType = "LectureNote"
                    }
                },
                Literatures = new List<SubjectLiteratureRowDto>
                {
                    new()
                    {
                        Type = "Əsas",
                        Author = "Seed Müəllif",
                        BookName = "Əsas dərsliyi (nümunə)",
                        Publisher = "Seed nəşriyyat",
                        PublicationYear = 2024
                    },
                    new()
                    {
                        Type = "Köməkçi",
                        Author = "Komissiya",
                        BookName = "Metodiki tövsiyələr",
                        Publisher = "-",
                        PublicationYear = 2023
                    }
                }
            };
        }

        private async Task SeedSubjectsAsync()
        {
            var departments = departmentRepository.GetAll().OrderBy(d => d.Id).ToList();

            foreach (var dept in departments)
            {
                if (subjectRepository.GetAll().Any(s => s.DepartmentId == dept.Id))
                    continue;

                try
                {
                    var request = BuildSeedSubjectAddRequest(dept.Id, dept.Name);
                    await mediator.Send(request).ConfigureAwait(false);
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Subject seed skipped (duplicate) department {DeptId}", dept.Id);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Subject seed failed for department {DeptId}", dept.Id);
                }
            }
        }

        /// <summary>
        /// Idempotent: subjects created by older seeders (name + department only) get sample topics, materials, and literature.
        /// </summary>
        private async Task SeedSubjectContentsAsync()
        {
            foreach (var subject in subjectRepository.GetAll().OrderBy(s => s.Id).ToList())
            {
                if (subjectTopicRepository.GetAll(t => t.SubjectId == subject.Id).Any())
                    continue;

                try
                {
                    var request = BuildSeedSubjectAddRequest(subject.DepartmentId, subject.Name);
                    foreach (var row in request.Topics)
                    {
                        await subjectTopicRepository.AddAsync(new SubjectTopic
                        {
                            SubjectId = subject.Id,
                            WeekNumber = row.WeekNumber,
                            TopicName = row.TopicName,
                            TeachingMethods = row.TeachingMethods,
                            Materials = row.Materials,
                            Equipment = row.Equipment
                        }, CancellationToken.None).ConfigureAwait(false);
                    }

                    foreach (var row in request.Materials)
                    {
                        await subjectMaterialRepository.AddAsync(new SubjectMaterial
                        {
                            SubjectId = subject.Id,
                            Title = row.Title,
                            Description = row.Description,
                            FileUrl = row.FileUrl,
                            MaterialType = row.MaterialType
                        }, CancellationToken.None).ConfigureAwait(false);
                    }

                    foreach (var row in request.Literatures)
                    {
                        await subjectLiteratureRepository.AddAsync(new SubjectLiterature
                        {
                            SubjectId = subject.Id,
                            Type = row.Type,
                            Author = row.Author,
                            BookName = row.BookName,
                            Publisher = row.Publisher,
                            PublicationYear = row.PublicationYear
                        }, CancellationToken.None).ConfigureAwait(false);
                    }

                    await subjectTopicRepository.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                    logger.LogInformation(
                        "Subject content seed applied for subject {SubjectId} ({SubjectName}).",
                        subject.Id,
                        subject.Name);
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Subject content seed skipped (duplicate) subject {SubjectId}", subject.Id);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Subject content seed failed for subject {SubjectId}", subject.Id);
                }
            }
        }

        private async Task SeedLessonsAsync()
        {
            var departments = departmentRepository.GetAll().OrderBy(d => d.Id).ToList();

            foreach (var dept in departments)
            {
                var subject = subjectRepository.GetAll()
                    .FirstOrDefault(s => s.DepartmentId == dept.Id);
                if (subject is null)
                    continue;

                var teacher = teacherRepository.GetAll()
                    .FirstOrDefault(t => t.TeacherDepartments.Any(td => td.DepartmentId == dept.Id));
                if (teacher is null)
                    continue;

                if (lessonRepository.GetAll().Any(l =>
                        l.TeacherId == teacher.Id && l.SubjectId == subject.Id))
                    continue;

                try
                {
                    await mediator.Send(new LessonAddRequest
                    {
                        TeacherId = teacher.Id,
                        SubjectId = subject.Id
                    }).ConfigureAwait(false);
                }
                catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                {
                    logger.LogInformation(ex, "Lesson seed skipped (duplicate) dept {DeptId}", dept.Id);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Lesson seed failed dept {DeptId}", dept.Id);
                }
            }
        }

        private async Task SeedLessonSchedulesAsync()
        {
            var roomIds = roomRepository.GetAll()
                .Where(r => r.Number > 0)
                .OrderBy(r => r.Id)
                .Select(r => r.Id)
                .ToList();

            if (roomIds.Count == 0)
            {
                logger.LogInformation("LessonSchedule seed skipped: no rooms with Number > 0.");
                return;
            }

            // Mix week parity so portal Üst/Alt filters show real differences (admin can still edit later).
            var templates = new (DayOfWeek Dow, TimeSpan Start, TimeSpan End, LessonType Type, WeekType Week)[]
            {
                (DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(10, 30, 0), LessonType.Mühazirə, WeekType.Both),
                (DayOfWeek.Wednesday, new TimeSpan(11, 0, 0), new TimeSpan(12, 30, 0), LessonType.Laboratoriya, WeekType.Upper),
                (DayOfWeek.Friday, new TimeSpan(14, 0, 0), new TimeSpan(15, 30, 0), LessonType.Məşğələ, WeekType.Lower)
            };

            var groups = groupRepository.GetAll().OrderBy(g => g.Id).ToList();

            foreach (var group in groups)
            {
                if (lessonScheduleRepository.GetAll().Any(s => s.GroupId == group.Id))
                    continue;

                var subject = subjectRepository.GetAll()
                    .FirstOrDefault(s => s.DepartmentId == group.DepartmentId);
                if (subject is null)
                    continue;

                var lesson = lessonRepository.GetAll()
                    .FirstOrDefault(l => l.SubjectId == subject.Id);
                if (lesson is null)
                    continue;

                var roomIndex = 0;
                foreach (var slot in templates)
                {
                    var roomId = roomIds[roomIndex % roomIds.Count];
                    roomIndex++;

                    try
                    {
                        await mediator.Send(new LessonScheduleAddRequest
                        {
                            LessonId = lesson.Id,
                            GroupId = group.Id,
                            DayOfWeek = slot.Dow,
                            StartTime = slot.Start,
                            EndTime = slot.End,
                            RoomId = roomId,
                            LessonType = slot.Type,
                            WeekType = slot.Week
                        }).ConfigureAwait(false);
                    }
                    catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                    {
                        logger.LogInformation(
                            ex,
                            "LessonSchedule seed skipped (duplicate) group {GroupId} {Day}",
                            group.Id,
                            slot.Dow);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            ex,
                            "LessonSchedule seed failed group {GroupId} {Day}",
                            group.Id,
                            slot.Dow);
                    }
                }
            }
        }

        /// <summary>
        /// Links each department group to that department's primary lesson (portal group counts, mixed lessons).
        /// </summary>
        private async Task SeedLessonGroupsAsync()
        {
            var groups = await db.Groups.AsNoTracking().OrderBy(g => g.Id).ToListAsync().ConfigureAwait(false);
            foreach (var group in groups)
            {
                var subject = await db.Subjects
                    .OrderBy(s => s.Id)
                    .FirstOrDefaultAsync(s => s.DepartmentId == group.DepartmentId)
                    .ConfigureAwait(false);
                if (subject is null)
                    continue;

                var lesson = await db.Lessons
                    .OrderBy(l => l.Id)
                    .FirstOrDefaultAsync(l => l.SubjectId == subject.Id)
                    .ConfigureAwait(false);
                if (lesson is null)
                    continue;

                var exists = await db.LessonGroups.AnyAsync(lg => lg.LessonId == lesson.Id && lg.GroupId == group.Id)
                    .ConfigureAwait(false);
                if (exists)
                    continue;

                db.LessonGroups.Add(new LessonGroup { LessonId = lesson.Id, GroupId = group.Id });
            }

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
            {
                logger.LogInformation(ex, "LessonGroup seed skipped (duplicate).");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "LessonGroup seed failed.");
            }
        }

        /// <summary>
        /// Backfills subject header fields (teachers, counts, purpose) when older rows were created empty.
        /// </summary>
        private async Task SeedSubjectCatalogMetadataAsync()
        {
            var subjects = await db.Subjects
                .Include(s => s.Department)
                .OrderBy(s => s.Id)
                .ToListAsync()
                .ConfigureAwait(false);

            var changed = false;
            foreach (var subject in subjects)
            {
                if (subject.Department is null)
                    continue;

                var template = BuildSeedSubjectAddRequest(subject.DepartmentId, subject.Department.Name);
                void FillIfEmpty()
                {
                    if (string.IsNullOrWhiteSpace(subject.LectureTeacher))
                    {
                        subject.LectureTeacher = template.LectureTeacher;
                        changed = true;
                    }

                    if (string.IsNullOrWhiteSpace(subject.SeminarTeacher))
                    {
                        subject.SeminarTeacher = template.SeminarTeacher;
                        changed = true;
                    }

                    if (string.IsNullOrWhiteSpace(subject.LabTeacher))
                    {
                        subject.LabTeacher = template.LabTeacher;
                        changed = true;
                    }

                    if (subject.StudentCount <= 0)
                    {
                        subject.StudentCount = template.StudentCount;
                        changed = true;
                    }

                    if (subject.Credits <= 0)
                    {
                        subject.Credits = template.Credits;
                        changed = true;
                    }

                    if (subject.TotalHours <= 0)
                    {
                        subject.TotalHours = template.TotalHours;
                        changed = true;
                    }

                    if (subject.WeekCount <= 0)
                    {
                        subject.WeekCount = template.WeekCount;
                        changed = true;
                    }

                    if (string.IsNullOrWhiteSpace(subject.Purpose))
                    {
                        subject.Purpose = template.Purpose;
                        changed = true;
                    }

                    if (string.IsNullOrWhiteSpace(subject.TeacherMethods))
                    {
                        subject.TeacherMethods = template.TeacherMethods;
                        changed = true;
                    }

                    if (string.IsNullOrWhiteSpace(subject.SyllabusUrl))
                    {
                        subject.SyllabusUrl = template.SyllabusUrl;
                        changed = true;
                    }

                    if (subject.FreeWorkScore <= 0 && template.FreeWorkScore > 0)
                    {
                        subject.FreeWorkScore = template.FreeWorkScore;
                        changed = true;
                    }

                    if (subject.SeminarScore <= 0 && template.SeminarScore > 0)
                    {
                        subject.SeminarScore = template.SeminarScore;
                        changed = true;
                    }

                    if (subject.LabScore <= 0 && template.LabScore > 0)
                    {
                        subject.LabScore = template.LabScore;
                        changed = true;
                    }

                    if (subject.AttendanceScore <= 0 && template.AttendanceScore > 0)
                    {
                        subject.AttendanceScore = template.AttendanceScore;
                        changed = true;
                    }

                    if (subject.ExamScore <= 0 && template.ExamScore > 0)
                    {
                        subject.ExamScore = template.ExamScore;
                        changed = true;
                    }

                    if (string.IsNullOrWhiteSpace(subject.Term))
                    {
                        subject.Term = template.Term ?? "2026 Yaz";
                        changed = true;
                    }

                    if (string.IsNullOrWhiteSpace(subject.GroupName))
                    {
                        subject.GroupName = template.GroupName ?? $"SEED-D{subject.DepartmentId}-qrup";
                        changed = true;
                    }
                }

                FillIfEmpty();
            }

            if (!changed)
                return;

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Subject catalog metadata seed failed.");
            }
        }

        /// <summary>
        /// Adds more weekly slots per group (Tue/Thu/Sat) when the base Mon/Wed/Fri set already exists.
        /// </summary>
        private async Task SeedAdditionalLessonSchedulesAsync()
        {
            var roomIds = await db.Rooms.AsNoTracking()
                .Where(r => r.Number > 0)
                .OrderBy(r => r.Id)
                .Select(r => r.Id)
                .ToListAsync()
                .ConfigureAwait(false);

            if (roomIds.Count == 0)
                return;

            var extras = new (DayOfWeek Dow, TimeSpan Start, TimeSpan End, LessonType Type, WeekType Week)[]
            {
                (DayOfWeek.Tuesday, new TimeSpan(10, 0, 0), new TimeSpan(11, 30, 0), LessonType.Mühazirə, WeekType.Both),
                (DayOfWeek.Thursday, new TimeSpan(13, 0, 0), new TimeSpan(14, 30, 0), LessonType.Məşğələ, WeekType.Upper),
                (DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(10, 30, 0), LessonType.Laboratoriya, WeekType.Lower)
            };

            var groups = await db.Groups.AsNoTracking().OrderBy(g => g.Id).ToListAsync().ConfigureAwait(false);
            var roomIndex = 0;

            foreach (var group in groups)
            {
                var subject = await db.Subjects
                    .OrderBy(s => s.Id)
                    .FirstOrDefaultAsync(s => s.DepartmentId == group.DepartmentId)
                    .ConfigureAwait(false);
                if (subject is null)
                    continue;

                var lesson = await db.Lessons
                    .OrderBy(l => l.Id)
                    .FirstOrDefaultAsync(l => l.SubjectId == subject.Id)
                    .ConfigureAwait(false);
                if (lesson is null)
                    continue;

                foreach (var slot in extras)
                {
                    var exists = await db.LessonSchedules.AnyAsync(ls =>
                            ls.LessonId == lesson.Id
                            && ls.GroupId == group.Id
                            && ls.DayOfWeek == slot.Dow
                            && ls.StartTime == slot.Start
                            && ls.WeekType == slot.Week)
                        .ConfigureAwait(false);
                    if (exists)
                        continue;

                    var roomId = roomIds[roomIndex % roomIds.Count];
                    roomIndex++;

                    try
                    {
                        await mediator.Send(new LessonScheduleAddRequest
                        {
                            LessonId = lesson.Id,
                            GroupId = group.Id,
                            DayOfWeek = slot.Dow,
                            StartTime = slot.Start,
                            EndTime = slot.End,
                            RoomId = roomId,
                            LessonType = slot.Type,
                            WeekType = slot.Week
                        }).ConfigureAwait(false);
                    }
                    catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
                    {
                        logger.LogInformation(
                            ex,
                            "Extra LessonSchedule skipped (duplicate) group {GroupId} {Day}",
                            group.Id,
                            slot.Dow);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            ex,
                            "Extra LessonSchedule failed group {GroupId} {Day}",
                            group.Id,
                            slot.Dow);
                    }
                }
            }
        }

        private static DateTime MostRecentOccurrenceOnOrBefore(DayOfWeek dow, DateTime todayUtcDate)
        {
            var diff = ((int)todayUtcDate.DayOfWeek - (int)dow + 7) % 7;
            return todayUtcDate.Date.AddDays(-diff);
        }

        /// <summary>
        /// Seeds attendance for past weeks. The <b>most recent session</b> of the <b>highest LessonSchedule Id</b>
        /// is left without rows so a teacher can open attendance and mark it manually.
        /// </summary>
        private async Task SeedAttendanceSessionsAsync()
        {
            var schedules = await db.LessonSchedules
                .AsNoTracking()
                .Include(ls => ls.Lesson)
                .Include(ls => ls.Group)
                    .ThenInclude(g => g.StudentGroups)
                .OrderBy(ls => ls.Id)
                .ToListAsync()
                .ConfigureAwait(false);

            if (schedules.Count == 0)
                return;

            var openScheduleId = schedules.Max(ls => ls.Id);
            var today = DateTime.UtcNow.Date;
            const int weeksOfHistory = 4;

            var toAdd = new List<Attendance>();

            foreach (var schedule in schedules)
            {
                if (schedule.Lesson is null || schedule.Group?.StudentGroups is null || schedule.Group.StudentGroups.Count == 0)
                    continue;

                var teacherId = schedule.Lesson.TeacherId;
                var sessionDates = new List<DateTime>();
                var anchor = MostRecentOccurrenceOnOrBefore(schedule.DayOfWeek, today);
                for (var i = 0; i < weeksOfHistory; i++)
                    sessionDates.Add(anchor.AddDays(-7 * i));

                foreach (var sessionDate in sessionDates)
                {
                    if (schedule.Id == openScheduleId && sessionDate == sessionDates[0])
                        continue;

                    var norm = sessionDate.Date;
                    var studentLinks = schedule.Group.StudentGroups.ToList();
                    for (var si = 0; si < studentLinks.Count; si++)
                    {
                        var sg = studentLinks[si];
                        var exists = await attendanceRepository
                            .GetByUniqueKeyAsync(schedule.Id, sg.StudentId, norm, CancellationToken.None)
                            .ConfigureAwait(false);
                        if (exists is not null)
                            continue;

                        var markedAt = norm.Add(schedule.StartTime);
                        if (markedAt.Kind == DateTimeKind.Unspecified)
                            markedAt = DateTime.SpecifyKind(markedAt, DateTimeKind.Utc);

                        toAdd.Add(new Attendance
                        {
                            StudentId = sg.StudentId,
                            LessonScheduleId = schedule.Id,
                            SessionDate = norm,
                            Status = si % 3 == 0 ? AttendanceStatus.Absent : AttendanceStatus.Present,
                            MarkedAt = markedAt,
                            LockAt = markedAt.AddDays(30),
                            IsLocked = true,
                            MarkedByTeacherId = teacherId
                        });
                    }
                }
            }

            if (toAdd.Count == 0)
                return;

            foreach (var row in toAdd)
                await attendanceRepository.AddAsync(row, CancellationToken.None).ConfigureAwait(false);

            try
            {
                await attendanceRepository.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                logger.LogInformation(
                    "Attendance seed added {Count} rows; open session: LessonScheduleId {OpenId} (most recent week, no rows).",
                    toAdd.Count,
                    openScheduleId);
            }
            catch (Exception ex) when (IsSqlUniqueOrDuplicateKey(ex))
            {
                logger.LogInformation(ex, "Attendance seed skipped (duplicate keys).");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Attendance seed failed.");
            }
        }
    }
}
