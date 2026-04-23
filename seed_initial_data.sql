-- 1. Insert Categories
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'الأحوال المدنية') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321001', N'الأحوال المدنية', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'المرور') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321002', N'المرور', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'التعليم') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321003', N'التعليم', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'الصحة') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321004', N'الصحة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'الشهر العقاري') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321005', N'الشهر العقاري', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'التجنيد') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321006', N'التجنيد', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'المرافق') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321007', N'المرافق', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'التأمينات الاجتماعية') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321008', N'التأمينات الاجتماعية', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'العمل') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321009', N'العمل', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'الجوازات') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321010', N'الجوازات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'خدمات حكومية متنوعة') INSERT INTO Categories (Id, Name, QuestionsCount, IsDeleted, CreatedAt) VALUES ('5E0B657A-5F9A-4E38-B5A1-987654321011', N'خدمات حكومية متنوعة', 0, 0, GETUTCDATE());

-- 2. Insert Tags
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'أرمل') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('bd07f98c-41a5-48bc-8f5a-a7e8dd70050f', N'أرمل', N'أرمل', N'أرمل', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'أفضل وقت') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('71d489f7-b38a-4b07-b32a-8484e604f307', N'أفضل وقت', N'أفضل وقت', N'أفضل-وقت', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'أوراق مطلوبة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('1a796408-7490-4ea9-a7e8-5b265922d787', N'أوراق مطلوبة', N'أوراق مطلوبة', N'أوراق-مطلوبة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'أول مرة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('199ac652-c7b1-4d4e-869f-07ad3ac654b5', N'أول مرة', N'أول مرة', N'أول-مرة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'أونلاين') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('2a772dcd-b8c9-444e-9d8d-0dfcd9fca306', N'أونلاين', N'أونلاين', N'أونلاين', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'إثبات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('d74eca3a-c7fd-4426-b9c1-428c6f1e9b7e', N'إثبات', N'إثبات', N'إثبات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'إثبات عنوان') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('a41b433e-8bcb-414f-95a9-2828b886e074', N'إثبات عنوان', N'إثبات عنوان', N'إثبات-عنوان', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'إجراءات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('ab92be65-a688-4f4b-aa9a-e16fdcaee049', N'إجراءات', N'إجراءات', N'إجراءات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'استعلام') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('a994bf8c-89a8-4765-84c6-9a720c601cf8', N'استعلام', N'استعلام', N'استعلام', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'العباسية') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('3a235372-37a6-48cc-84e7-ddbb11093e1f', N'العباسية', N'العباسية', N'العباسية', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'انتهاء صلاحية') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('854bf480-e865-4226-976f-80ee4310f3a5', N'انتهاء صلاحية', N'انتهاء صلاحية', N'انتهاء-صلاحية', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'بدل فاقد') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('de49c788-a3fe-4db4-a48d-a1e612e634a5', N'بدل فاقد', N'بدل فاقد', N'بدل-فاقد', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'بطاقة رقم قومي') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('2fbc20e7-0660-42e0-be33-5e643549e3f9', N'بطاقة رقم قومي', N'بطاقة رقم قومي', N'بطاقة-رقم-قومي', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'بوابة حكومية') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('6dc7ac3d-02db-4601-8617-762965fb5ef2', N'بوابة حكومية', N'بوابة حكومية', N'بوابة-حكومية', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'بيان نجاح') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('81bdd0b3-8841-4892-9d16-88f5c3f8a653', N'بيان نجاح', N'بيان نجاح', N'بيان-نجاح', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'بيانات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('5cbb33ed-a94e-4ca5-8602-6804ca5c6eec', N'بيانات', N'بيانات', N'بيانات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تأخير') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('cf84c49e-c7db-4079-a58a-abcb36563610', N'تأخير', N'تأخير', N'تأخير', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تجديد بطاقة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('d844b7bd-a4eb-4ea1-bfa0-53a49e3b5fb4', N'تجديد بطاقة', N'تجديد بطاقة', N'تجديد-بطاقة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تجديد رخصة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('03fca9e1-ab5f-49ee-b7c8-8e518fe4aa9e', N'تجديد رخصة', N'تجديد رخصة', N'تجديد-رخصة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تراخيص') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('c0faeb41-237e-46b9-9032-2628f7198175', N'تراخيص', N'تراخيص', N'تراخيص', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تعديل بيانات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('57789f48-4707-479c-bc1f-7d50d33c1d50', N'تعديل بيانات', N'تعديل بيانات', N'تعديل-بيانات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تغيير حالة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('1ac4df5b-d40a-4463-8716-b86a2da96d93', N'تغيير حالة', N'تغيير حالة', N'تغيير-حالة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تغيير عنوان') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('d20ffba1-1b79-4e36-9cc2-78e362567bf4', N'تغيير عنوان', N'تغيير عنوان', N'تغيير-عنوان', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تغيير مهنة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('0b38c822-9541-4bd1-b995-f6dbb2d67832', N'تغيير مهنة', N'تغيير مهنة', N'تغيير-مهنة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'تكلفة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('0c4d997f-82d5-4781-a307-fc2083e10ff7', N'تكلفة', N'تكلفة', N'تكلفة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'توثيق') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('5f3800bd-4556-4e63-af31-3e44aca3a382', N'توثيق', N'توثيق', N'توثيق', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'توكيل') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('ef0fcbc0-b32a-49f7-ab4c-b05ecd71a348', N'توكيل', N'توكيل', N'توكيل', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'جامعة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('ca293769-44cf-4dd3-a4b3-75c31173e548', N'جامعة', N'جامعة', N'جامعة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'حالة اجتماعية') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('ae71f43d-a68b-48cd-b23f-55fe74997506', N'حالة اجتماعية', N'حالة اجتماعية', N'حالة-اجتماعية', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'حضور الزوج') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('fe66db09-5d51-4b22-87fa-364bb86c55c7', N'حضور الزوج', N'حضور الزوج', N'حضور-الزوج', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'حضور شخصي') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('e2064e22-c4b4-4e0f-971d-5bbb221eb058', N'حضور شخصي', N'حضور شخصي', N'حضور-شخصي', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'خدمات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('419eefcd-a7c2-4c1d-9d25-53656195e613', N'خدمات', N'خدمات', N'خدمات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'خدمة مستعجلة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('f3bab509-0fc5-4ff3-98d4-5be9dbe6fa5d', N'خدمة مستعجلة', N'خدمة مستعجلة', N'خدمة-مستعجلة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'رخصة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('f031d7c5-d459-406d-a76c-630faf3db281', N'رخصة', N'رخصة', N'رخصة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'رخصة قيادة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('b250a103-faf6-4e3d-97a4-2fa78fa60f04', N'رخصة قيادة', N'رخصة قيادة', N'رخصة-قيادة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'رسوم') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('ed02162e-8353-46f8-a001-26910b9a0d58', N'رسوم', N'رسوم', N'رسوم', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'رقم قومي') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('3ca42456-9ed8-4e75-ba31-ffe796f4b582', N'رقم قومي', N'رقم قومي', N'رقم-قومي', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'زحمة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('5d235a92-f3e9-4783-b94a-d02e46b661e7', N'زحمة', N'زحمة', N'زحمة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'سجل تجاري') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('bce5f04a-af17-4ec5-a0aa-8cf7c235da5c', N'سجل تجاري', N'سجل تجاري', N'سجل-تجاري', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'سجل مدني') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('e9649ec6-79b2-4743-a8c8-e513b0b5dd17', N'سجل مدني', N'سجل مدني', N'سجل-مدني', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'سرعة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('71c1a2e7-833b-419b-a5e3-a7764d39f9c5', N'سرعة', N'سرعة', N'سرعة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'سعر') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('07ac49f5-c13f-406f-a0f7-48a01afe6d51', N'سعر', N'سعر', N'سعر', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'شهادة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('b651b2ef-a94a-4e45-8e19-3fa16d413d8d', N'شهادة', N'شهادة', N'شهادة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'شهادة تخرج') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('ef6819e1-ee4d-48cf-aed3-497953018ec2', N'شهادة تخرج', N'شهادة تخرج', N'شهادة-تخرج', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'شهادة مزاولة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('f451cc26-9851-4ed6-a22b-e8aaa738797a', N'شهادة مزاولة', N'شهادة مزاولة', N'شهادة-مزاولة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'ضائع') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('68e4c347-fd6f-4ea7-9b2f-2fd0ae1d04a6', N'ضائع', N'ضائع', N'ضائع', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'عقد إيجار') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('ad12b98f-656e-46e6-a2e9-4bd0233353d3', N'عقد إيجار', N'عقد إيجار', N'عقد-إيجار', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'عنوان ثابت') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('9f412a02-4579-4874-9e67-1ed24172ef32', N'عنوان ثابت', N'عنوان ثابت', N'عنوان-ثابت', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'غرامة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('3427abdc-a7cd-40cb-b12c-3929e7e48183', N'غرامة', N'غرامة', N'غرامة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'فرق') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('64dd912b-d6e1-4f65-9d5d-0ac9c7eb2062', N'فرق', N'فرق', N'فرق', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيادة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('364381fe-6df8-4db5-bbb6-b8958adb40ed', N'قيادة', N'قيادة', N'قيادة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيد عائلي') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('f93767a9-7048-46ca-8d13-265cda7242b6', N'قيد عائلي', N'قيد عائلي', N'قيد-عائلي', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيد فردي') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('fd66751e-875b-4cf9-97c2-99e51215710c', N'قيد فردي', N'قيد فردي', N'قيد-فردي', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'متزوج') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('9a6e7832-7282-4b39-8919-6dcba38d76e3', N'متزوج', N'متزوج', N'متزوج', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'محل') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('e0df30ef-958e-4dbb-a961-f8b69f0373e7', N'محل', N'محل', N'محل', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مخالفات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('fb2644b6-8850-4652-a637-8e9c866f6b3c', N'مخالفات', N'مخالفات', N'مخالفات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مدة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('34f45c28-b3f7-43fb-9e3a-9d6059b4334f', N'مدة', N'مدة', N'مدة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مدة استخراج') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('1e9ab669-8c1e-4bb1-9383-896df03afeac', N'مدة استخراج', N'مدة استخراج', N'مدة-استخراج', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مدرسة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('5f565df7-550b-442a-b594-e8fba691f9e7', N'مدرسة', N'مدرسة', N'مدرسة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مرور') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('f72018ce-6735-4097-ad0f-b58fdb6d25c4', N'مرور', N'مرور', N'مرور', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مستندات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('a6658ab1-9966-4e1a-9efa-f2d2063e432f', N'مستندات', N'مستندات', N'مستندات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مطلق') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('d86f174a-b2c3-4a08-ab0b-c5dd2b64c452', N'مطلق', N'مطلق', N'مطلق', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مكتب القاهرة') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('c6225688-7b1b-4a19-b5e7-9a2cedbc9d0c', N'مكتب القاهرة', N'مكتب القاهرة', N'مكتب-القاهرة', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مهنة حر') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('0e79310b-c5f2-4291-a450-d6677f872a10', N'مهنة حر', N'مهنة حر', N'مهنة-حر', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مواعيد') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('19334355-635e-451e-abbe-a6f8845d3575', N'مواعيد', N'مواعيد', N'مواعيد', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'مولات') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('c294cbe6-1829-4930-98cd-a6f29e8bd651', N'مولات', N'مولات', N'مولات', 0, 0, GETUTCDATE());
IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = N'وقت') INSERT INTO Tags (Id, Name, NormalizedName, Slug, UsageCount, IsDeleted, CreatedAt) VALUES ('7d5dd603-07a9-41a8-bdce-c689d1a3097b', N'وقت', N'وقت', N'وقت', 0, 0, GETUTCDATE());

-- 3. Insert Questions & Answers

-- Question: ازاي أطلع بطاقة رقم قومي لأول مرة؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'f47ac10b-58cc-4372-a567-0e02b2c3d479')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('f47ac10b-58cc-4372-a567-0e02b2c3d479', N'ازاي أطلع بطاقة رقم قومي لأول مرة؟', N'ازاي أطلع بطاقة رقم قومي لأول مرة؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('550e8400-e29b-41d4-a716-446655440000', N'روح أقرب مكتب سجل مدني تابع لمحل إقامتك. هتحتاج أصل شهادة الميلاد + صورة منها، وصورة شخصية حديثة بيضاء، وبطاقة ولي الأمر (الأب أو الأم). بتتكلف حوالي 50 جنيهاً، وتستلمها بعد 15 يوم. (الموقع: الإسكندرية)', 'f47ac10b-58cc-4372-a567-0e02b2c3d479', '00000000-0000-0000-0000-000000000001', 0, 12, 0, 0, 24, 0, '2026-03-15');
END

-- Question: ايه الأوراق المطلوبة لتجديد البطاقة؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = '7c9e6679-7425-40de-944b-e07fc1f90ae7')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('7c9e6679-7425-40de-944b-e07fc1f90ae7', N'ايه الأوراق المطلوبة لتجديد البطاقة؟', N'ايه الأوراق المطلوبة لتجديد البطاقة؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('6ba7b810-9dad-11d1-80b4-00c04fd430c8', N'الأوراق: البطاقة القديمة (حتى لو منتهية)، وصورة شخصية حديثة خلفية بيضاء، ورسوم 50 جنيه. لو البطاقة قديمة جداً أو تالفة، هتحتاج تقديم إقرار فقدان وتدفع غرامة صغيرة حوالي 20 جنيهاً. (الموقع: القاهرة)', '7c9e6679-7425-40de-944b-e07fc1f90ae7', '00000000-0000-0000-0000-000000000001', 0, 8, 0, 0, 16, 0, '2026-03-10');
END

-- Question: هل ينفع أطلع البطاقة من غير عنوان ثابت؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c', N'هل ينفع أطلع البطاقة من غير عنوان ثابت؟', N'هل ينفع أطلع البطاقة من غير عنوان ثابت؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('b1c2d3e4-f5a6-7b8c-9d0e-1f2a3b4c5d6e', N'للأسف، لازم يكون عندك عنوان ثابت وموثق. هتحتاج تقديم عقد إيجار موثق أو إيصال مرافق (كهرباء أو غاز) باسمك أو اسم رب الأسرة. غير كده، هيرفضوا استخراج البطاقة أو تجديدها. (الموقع: الجيزة)', 'a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c', '00000000-0000-0000-0000-000000000001', 0, 15, 0, 0, 30, 0, '2026-03-05');
END

-- Question: مدة استخراج البطاقة كام يوم؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'd4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a', N'مدة استخراج البطاقة كام يوم؟', N'مدة استخراج البطاقة كام يوم؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('e5f6a7b8-c9d0-1e2f-3a4b-5c6d7e8f9a0b', N'الخدمة العادية: 15 يوم عمل. الخدمة المستعجلة: 3 أيام وبتكلف حوالي 150 جنيه بدل 50 جنيه. في مكاتب معينة في المولات بتطلعها في نفس اليوم لكن برسوم أعلى (250-300 جنيه). (الموقع: القاهرة)', 'd4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a', '00000000-0000-0000-0000-000000000001', 0, 20, 0, 0, 40, 0, '2026-03-12');
END

-- Question: هل فيه خدمة مستعجلة للبطاقة؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c', N'هل فيه خدمة مستعجلة للبطاقة؟', N'هل فيه خدمة مستعجلة للبطاقة؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('a7b8c9d0-e1f2-3a4b-5c6d-7e8f9a0b1c2d', N'أيوة، في خدمة الاستعجال بـ 150 جنيه بدل 50 جنيه، بتستلمها خلال 3 أيام عمل. فيه كمان خدمة 'استلام فوري' في بعض مراكز الخدمة في المولات التجارية الكبيرة (زي City Stars أو Mall of Arabia) لكن برسوم أعلى تصل لـ 300 جنيه. (الموقع: الإسكندرية)', 'f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c', '00000000-0000-0000-0000-000000000001', 0, 5, 0, 0, 10, 0, '2026-03-18');
END

-- Question: ايه الفرق بين السجل المدني والمولات؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', N'ايه الفرق بين السجل المدني والمولات؟', N'ايه الفرق بين السجل المدني والمولات؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('c9d0e1f2-a3b4-5678-9012-cdef12345678', N'السجل المدني: تابع للحكومة، رسومه أقل، بس زحمة كتير وخدمة أبطأ. المولات: مراكز خدمة خاصة متعاقدة مع الحكومة، بتقدم خدمة أسرع (نفس اليوم أو اليومين) وفي أماكن مكيفة وانتظار أقل، لكن الرسوم أغلى بكتير (ضعف أو تلاتة أضعاف). (الموقع: القاهرة)', 'b8c9d0e1-f2a3-4567-8901-bcdef1234567', '00000000-0000-0000-0000-000000000001', 0, 10, 0, 0, 20, 0, '2026-03-20');
END

-- Question: هل ينفع أغير المهنة في البطاقة؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'd0e1f2a3-b4c5-6789-0123-def123456789')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('d0e1f2a3-b4c5-6789-0123-def123456789', N'هل ينفع أغير المهنة في البطاقة؟', N'هل ينفع أغير المهنة في البطاقة؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('e1f2a3b4-c5d6-7e8f-9a0b-1c2d3e4f5a6b', N'أيوة، بس لازم تقدم مستند رسمي يثبت المهنة الجديدة. لو موظف: خطاب من جهة العمل مختوم بختم النسر. لو صاحب مهنة حرة: كارنيه النقابة أو شهادة مزاولة المهنة. لو عاطل: هتثبت 'عاطل' أو 'رب منزل' بدون مستندات. (الموقع: الجيزة)', 'd0e1f2a3-b4c5-6789-0123-def123456789', '00000000-0000-0000-0000-000000000001', 0, 7, 0, 0, 14, 0, '2026-03-08');
END

-- Question: ازاي أثبت المهنة لو شغال حر؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c', N'ازاي أثبت المهنة لو شغال حر؟', N'ازاي أثبت المهنة لو شغال حر؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('a3b4c5d6-e7f8-9a0b-1c2d-3e4f5a6b7c8d', N'لو شغال حر (مثلاً: كهربائي، سباك، مصور)، روح الوحدة المحلية أو الحي التابع له وقدم إقرار مزاولة مهنة. ممكن تطلع كارنيه مزاولة مهنة من الغرفة التجارية أو اتحاد الصناعات حسب نشاطك. من غير المستندات دي، مش هيعترفوا بمهنتك. (الموقع: القاهرة)', 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c', '00000000-0000-0000-0000-000000000001', 0, 9, 0, 0, 18, 0, '2026-03-22');
END

-- Question: هل لازم عقد إيجار موثق لتغيير العنوان؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e', N'هل لازم عقد إيجار موثق لتغيير العنوان؟', N'هل لازم عقد إيجار موثق لتغيير العنوان؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('c5d6e7f8-a9b0-1c2d-3e4f-5a6b7c8d9e0f', N'أيوة، لازم عقد إيجار موثق من الشهر العقاري. أو ممكن تقدم إيصال مرافق (كهرباء أو غاز) حديث باسمك أو اسم رب الأسرة (مع إثبات صلة القرابة زي بطاقة العائلة أو شهادة ميلاد). الفواتير الإلكترونية أو صورها مقبولة. (الموقع: الإسكندرية)', 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e', '00000000-0000-0000-0000-000000000001', 0, 14, 0, 0, 28, 0, '2026-03-01');
END

-- Question: تكلفة استخراج البطاقة كام؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('d6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a', N'تكلفة استخراج البطاقة كام؟', N'تكلفة استخراج البطاقة كام؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('e7f8a9b0-c1d2-3e4f-5a6b-7c8d9e0f1a2b', N'الرسوم الرسمية: 50 جنيهاً للاستخراج العادي (15 يوم). الخدمة المستعجلة: 150 جنيهاً (3 أيام). لو بدل فاقد أو تالف: 50 جنيهاً + غرامة 20 جنيهاً لو قدمت إقرار فقدان. رسوم التوصيل للبيت: 30 جنيهاً إضافية. (الموقع: القاهرة)', 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a', '00000000-0000-0000-0000-000000000001', 0, 25, 0, 0, 50, 0, '2026-03-14');
END

-- Question: هل فيه غرامة لو البطاقة منتهية؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c', N'هل فيه غرامة لو البطاقة منتهية؟', N'هل فيه غرامة لو البطاقة منتهية؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('a9b0c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d', N'أيوة، لو البطاقة منتهية من أكثر من 6 شهور، هتدفع غرامة 20 جنيهاً عن كل سنة تأخير (بحد أقصى 100 جنيه). لو منتهية من أقل من 6 شهور، بتجدد عادي بدون غرامة. الأفضل متأخرش عشان تتعرض للمساءلة لو موظف حكومي طلبها منك. (الموقع: الجيزة)', 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c', '00000000-0000-0000-0000-000000000001', 0, 11, 0, 0, 22, 0, '2026-03-09');
END

-- Question: ازاي أطلع بدل فاقد؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e', N'ازاي أطلع بدل فاقد؟', N'ازاي أطلع بدل فاقد؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('c1d2e3f4-a5b6-7c8d-9e0f-1a2b3c4d5e6f', N'روح أقرب سجل مدني، قدم إقرار فقدان (بتكتب إن البطاقة ضاعت أو اترددت أو اتحرقت)، وادفع رسوم 50 جنيه + غرامة 20 جنيه. لازم تقدم إيصال مرافق أو عقد إيجار لإثبات العنوان عشان البطاقة الجديدة تطلع بنفس البيانات أو بالعنوان الجديد. (الموقع: القاهرة)', 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e', '00000000-0000-0000-0000-000000000001', 0, 6, 0, 0, 12, 0, '2026-03-17');
END

-- Question: هل لازم أروح بنفسي؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'd2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('d2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a', N'هل لازم أروح بنفسي؟', N'هل لازم أروح بنفسي؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('e3f4a5b6-c7d8-9012-3456-ef1237890123', N'أيوة، لازم تروح بنفسك عشان البصمة والصورة الشخصية والكشف البصري. مينفعش حد يعملك توكيل لاستخراج بطاقة رقم قومي لأول مرة أو التجديد، لأنها وثيقة هوية بيومترية. الاستلام ممكن أي حد بالبطاقة القديمة والإيصال. (الموقع: الإسكندرية)', 'd2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a', '00000000-0000-0000-0000-000000000001', 0, 18, 0, 0, 36, 0, '2026-03-21');
END

-- Question: ايه أفضل وقت أروح فيه السجل المدني؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c', N'ايه أفضل وقت أروح فيه السجل المدني؟', N'ايه أفضل وقت أروح فيه السجل المدني؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('a5b6c7d8-e9f0-1a2b-3c4d-5e6f7a8b9c0d', N'أحسن وقت تروح: أيام الأحد للثلاثاء من الساعة 8 الصبح لـ 10 الصبح. تجنب يوم الأربعاء (زحمة معاملات) والخميس (نص دوام). كمان تجنب أول وأخر أسبوع في الشهر عشان الموظفين والمواطنين بيخلصوا معاملات. مولات الخدمة أفضل من حيث الزحمة. (الموقع: القاهرة)', 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c', '00000000-0000-0000-0000-000000000001', 0, 22, 0, 0, 44, 0, '2026-03-19');
END

-- Question: هل فيه زحمة في العباسية؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'b6c7d8e9-f0a1-2345-6789-0abcdef12345')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('b6c7d8e9-f0a1-2345-6789-0abcdef12345', N'هل فيه زحمة في العباسية؟', N'هل فيه زحمة في العباسية؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('c7d8e9f0-a1b2-3c4d-5e6f-7a8b9c0d1e2f', N'للأسف، مكتب العباسية (مكتب القاهرة الكبرى) معروف بزحمة خانقة طول اليوم. لو مضطر تروحله، روح من 7 الصبح بالضبط أو استنى للعصر بعد الساعة 2. الأفضل تروح لأي مكتب تابع لحيك في مدينتك زي مدينة نصر أو مصر الجديدة أو المعادي هتخلص أسرع بكتير. (الموقع: القاهرة)', 'b6c7d8e9-f0a1-2345-6789-0abcdef12345', '00000000-0000-0000-0000-000000000001', 0, 30, 0, 0, 60, 0, '2026-03-23');
END

-- Question: ازاي أغير الحالة الاجتماعية؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a', N'ازاي أغير الحالة الاجتماعية؟', N'ازاي أغير الحالة الاجتماعية؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', N'لازم تقدم مستند يثبت التغيير: لو اتجوزت: وثيقة الزواج الأصلية (عقد الزواج). لو اتطلقت: وثيقة الطلاق النهائية. لو اتوفى الزوج/الزوجة: شهادة الوفاة. بتروح السجل المدني وتطلب تعديل الحالة الاجتماعية وبتدفع 50 جنيه رسوم بطاقة جديدة بالحالة المحدثة. (الموقع: الجيزة)', 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a', '00000000-0000-0000-0000-000000000001', 0, 13, 0, 0, 26, 0, '2026-03-07');
END

-- Question: هل لازم الزوج يحضر؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'f0a1b2c3-d4e5-6789-0123-f45678901234')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('f0a1b2c3-d4e5-6789-0123-f45678901234', N'هل لازم الزوج يحضر؟', N'هل لازم الزوج يحضر؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d', N'لو الست بتغير الحالة الاجتماعية من أعزب لمتزوج، مش لازم الزوج يحضر، هي بتقدم عقد الزواج بس. لو الزوج عايز يغير حالته لـ 'متزوج'، لازم هو يحضر شخصياً أو يقدم توكيل. لو الحالة 'مطلق' أو 'أرمل'، الشخص نفسه بس اللي بيحضر مع الوثيقة المطلوبة. (الموقع: الإسكندرية)', 'f0a1b2c3-d4e5-6789-0123-f45678901234', '00000000-0000-0000-0000-000000000001', 0, 4, 0, 0, 8, 0, '2026-03-11');
END

-- Question: ازاي أطلع قيد عائلي؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', N'ازاي أطلع قيد عائلي؟', N'ازاي أطلع قيد عائلي؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('b2c3d4e5-f6a7-8901-2345-67890abcdef1', N'روح مكتب السجل المدني التابع لمحل إقامتك. هتحتاج أصل بطاقة الرقم القومي لرب الأسرة (الأب أو الأم)، وشهادة ميلاد الأبناء (اللي لسه تحت السن)، ورسوم 10 جنيهات. بيطلع فوري أو في نفس اليوم. بيستخدم كإثبات للعلاقات العائلية في المدارس أو المستشفيات. (الموقع: القاهرة)', 'a1b2c3d4-e5f6-7890-1234-567890abcdef', '00000000-0000-0000-0000-000000000001', 0, 9, 0, 0, 18, 0, '2026-03-02');
END

-- Question: القيد العائلي بياخد وقت قد ايه؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'c3d4e5f6-a7b8-9012-3456-7890abcdef12')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('c3d4e5f6-a7b8-9012-3456-7890abcdef12', N'القيد العائلي بياخد وقت قد ايه؟', N'القيد العائلي بياخد وقت قد ايه؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('d4e5f6a7-b8c9-0123-4567-890abcdef123', N'في أغلب المكاتب بيطلع في نفس اليوم خلال ساعة أو ساعتين. لو المكتب زحمة، ممكن ياخد من يوم ليومين. لو طلبه من مكتب غير تابع لمحل إقامتك (مثلاً قاطن في القاهرة وعايز قيد من محافظة أسيوط)، هتستنى 3-5 أيام عشان ييجي من المحافظة التانية. (الموقع: الإسكندرية)', 'c3d4e5f6-a7b8-9012-3456-7890abcdef12', '00000000-0000-0000-0000-000000000001', 0, 14, 0, 0, 28, 0, '2026-03-05');
END

-- Question: ايه الفرق بين القيد الفردي والعائلي؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'e5f6a7b8-c9d0-1234-5678-90abcdef1234')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('e5f6a7b8-c9d0-1234-5678-90abcdef1234', N'ايه الفرق بين القيد الفردي والعائلي؟', N'ايه الفرق بين القيد الفردي والعائلي؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('f6a7b8c9-d0e1-2345-6789-0abcdef12345', N'القيد الفردي: بيبين حالة الشخص لوحده (اسمه، تاريخ ميلاده، مهنته، حالته الاجتماعية). القيد العائلي: بيبين رب الأسرة والزوجة والأبناء اللي لسه تحت السن (أقل من 18 سنة). القيد العائلي بيستخدم عشان تسجيل الأطفال في المدارس أو استخراج جوازات السفر للأسرة. (الموقع: الجيزة)', 'e5f6a7b8-c9d0-1234-5678-90abcdef1234', '00000000-0000-0000-0000-000000000001', 0, 21, 0, 0, 42, 0, '2026-03-08');
END

-- Question: هل ينفع أطلعه أونلاين؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'a7b8c9d0-e1f2-3456-7890-abcdef123456')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('a7b8c9d0-e1f2-3456-7890-abcdef123456', N'هل ينفع أطلعه أونلاين؟', N'هل ينفع أطلعه أونلاين؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321001', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', N'أيوة، تقدر تطلبه من خلال بوابة 'مصر الرقمية' (digital.egypt.gov.eg). بتسجل دخولك، وتختار 'قيد عائلي'، وتدفع الرسوم أونلاين (10 جنيهات)، وتستلمه على بريدك الإلكتروني كملف PDF مختوم إلكترونياً. بيطلع فوري وممكن تطبعه في البيت. (الموقع: القاهرة)', 'a7b8c9d0-e1f2-3456-7890-abcdef123456', '00000000-0000-0000-0000-000000000001', 0, 27, 0, 0, 54, 0, '2026-03-10');
END

-- Question: ازاي أطلع رخصة قيادة لأول مرة؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', N'ازاي أطلع رخصة قيادة لأول مرة؟', N'ازاي أطلع رخصة قيادة لأول مرة؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321002', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('f0a1b2c3-d4e5-6789-0123-f45678901234', N'أول حاجة تقدم في مدرسة تعليم القيادة المرخصة من المرور. بتاخد 12 درس نظري و12 درس عملي. بعد كده بتقدم لموعد الاختبار في إدارة المرور التابع لمحل إقامتك. الاختبار فيه جزء نظري (إشارات وقوانين) وجزء عملي (قيادة). بعد النجاح، بتدفع رسوم الرخصة: 250 جنيه (لسنة) أو 500 جنيه (لـ 10 سنين). هتحتاج: بطاقة رقم قومي، 6 صور شخصية، شهادة صحية من مستشفى حكومي، شهادة اجتياز من مدرسة القيادة. (الموقع: الجيزة)', 'e9f0a1b2-c3d4-5678-9012-ef4567890123', '00000000-0000-0000-0000-000000000001', 0, 24, 0, 0, 48, 0, '2026-04-05');
END

-- Question: الأوراق المطلوبة لتجديد الرخصة؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', N'الأوراق المطلوبة لتجديد الرخصة؟', N'الأوراق المطلوبة لتجديد الرخصة؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321002', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('b2c3d4e5-f6a7-8901-2345-67890abcdef1', N'الأوراق: 1) الرخصة القديمة (حتى لو منتهية). 2) بطاقة الرقم القومي سارية. 3) شهادة طبية معتمدة من مستشفى حكومي أو مركز طبي معتمد (بتتكلف 50-100 جنيه). 4) صورة شخصية حديثة خلفية بيضاء. 5) رسوم التجديد: 250 جنيه لسنة، 500 لـ 10 سنين. بتقدمهم في أقرب إدارة مرور تابع لمحل إقامتك. الأفضل تجدد قبل ما تنتهي بـ 3 شهور عشان تتجنب الغرامة. (الموقع: القاهرة)', 'a1b2c3d4-e5f6-7890-1234-567890abcdef', '00000000-0000-0000-0000-000000000001', 0, 18, 0, 0, 36, 0, '2026-04-07');
END

-- Question: تكلفة استخراج الرخصة كام؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', N'تكلفة استخراج الرخصة كام؟', N'تكلفة استخراج الرخصة كام؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321002', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('c9d0e1f2-a3b4-5678-9012-cdef12345678', N'التكاليف التقريبية: رسوم الرخصة نفسها: 250 جنيه (سنة) أو 500 جنيه (10 سنين). الكشف الطبي: 50 جنيه. رسوم مدرسة القيادة: من 1000 لـ 2500 جنيه حسب المحافظة والمدرسة. الرسوم الإدارية في المرور: 35 جنيه. المجموع لأول مرة: حوالي 1500 جنيه لو معادتش غير مرة. التجديد بس: 250/500 + 50 كشف + 35 رسوم إدارية = 335 أو 585 جنيه. (الموقع: القاهرة)', 'b8c9d0e1-f2a3-4567-8901-bcdef1234567', '00000000-0000-0000-0000-000000000001', 0, 20, 0, 0, 40, 0, '2026-04-13');
END

-- Question: ازاي أعرف مخالفات المرور؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4567-8901-f78901234567')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('f8a9b0c1-d2e3-4567-8901-f78901234567', N'ازاي أعرف مخالفات المرور؟', N'ازاي أعرف مخالفات المرور؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321002', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('a9b0c1d2-e3f4-5678-9012-abcdef123789', N'أسهل طريقة: من خلال تطبيق 'مرورك' أو موقع 'البوابة المصرية الموحدة'. بتدخل رقم العربية أو رقم رخصة القيادة، بتظهر كل المخالفات المسجلة عليك. كمان تقدر تتصل بالرقم 01221110000 (خدمة عملاء المرور). أو تروح بنفسك لأي إدارة مرور وتطلب 'شهادة مخالفات'. لازم تتأكد قبل ما تجدد الرخصة عشان أي مخالفة هتمنع التجديد. (الموقع: القاهرة)', 'f8a9b0c1-d2e3-4567-8901-f78901234567', '00000000-0000-0000-0000-000000000001', 0, 35, 0, 0, 70, 0, '2026-04-23');
END

-- Question: ازاي أطلع بيان نجاح؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'c7d8e9f0-a1b2-3456-7890-cdef45678901')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('c7d8e9f0-a1b2-3456-7890-cdef45678901', N'ازاي أطلع بيان نجاح؟', N'ازاي أطلع بيان نجاح؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321003', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('d8e9f0a1-b2c3-4567-8901-def456789012', N'روح المدرسة اللي درست فيها آخر سنة. بتطلب بيان نجاح (أو شهادة مؤقتة) من شؤون الطلاب. هتحتاج بطاقة الرقم القومي (بتاعتك أو بتاعت ولي الأمر لو لسه تحت 18). الرسوم: 20 جنيه. بيطلع في نفس اليوم. لو المدرسة اتهدت أو اتغير اسمها، روح الإدارة التعليمية التابعة لها. لو عايز بيان نجاح للجامعة أو السفر، ممكن تطلبه أونلاين من موقع 'التنسيق الإلكتروني' التابع لوزارة التعليم العالي. (الموقع: الإسكندرية)', 'c7d8e9f0-a1b2-3456-7890-cdef45678901', '00000000-0000-0000-0000-000000000001', 0, 9, 0, 0, 18, 0, '2026-05-03');
END

-- Question: هل ينفع أستخرج شهادة تخرج بدل فاقد؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', N'هل ينفع أستخرج شهادة تخرج بدل فاقد؟', N'هل ينفع أستخرج شهادة تخرج بدل فاقد؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321003', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('f0a1b2c3-d4e5-6789-0123-f45678901234', N'أيوة، روح كليتك أو الجامعة اللي تخرجت منها. قدم طلب 'بدل فاقد لشهادة التخرج' مع إقرار فقدان (بتكتب إزاي ضاعت أو اترددت). هتحتاج بطاقتك، وصور شخصية، وتدفع رسوم 100-250 جنيه حسب الجامعة (حكومي أو خاص). بياخد من أسبوع لشهر عما يدوروا على الأصل في الأرشيف. لو عايز بدل تالف (ممزقة)، هات الشهادة التالفة معاك. (الموقع: القاهرة)', 'e9f0a1b2-c3d4-5678-9012-ef4567890123', '00000000-0000-0000-0000-000000000001', 0, 16, 0, 0, 32, 0, '2026-05-05');
END

-- Question: لتأسيس شركة أو محل تجاري، أول خطوة؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', N'لتأسيس شركة أو محل تجاري، أول خطوة؟', N'لتأسيس شركة أو محل تجاري، أول خطوة؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321011', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('c9d0e1f2-a3b4-5678-9012-cdef12345678', N'لتأسيس شركة أو محل تجاري، أول خطوة هي استخراج سجل تجاري من مكتب السجل التجاري التابع للغرفة التجارية. المستندات: عقد إيجار المحل موثق، بطاقة الرقم القومي، إيصال مرافق، صورة من العقد التأسيسي (لشركة). الرسوم: 500-1000 جنيه حسب نوع النشاط. بياخد أسبوعين. بعد السجل التجاري، تروح للضرائب تطلع بطاقة ضريبية، وللتأمينات تسجل العمال، وللسجل الصناعي لو نشاطك صناعي. (الموقع: الإسكندرية)', 'b8c9d0e1-f2a3-4567-8901-bcdef1234567', '00000000-0000-0000-0000-000000000001', 0, 18, 0, 0, 36, 0, '2026-08-11');
END

-- Question: للاستعلام عن بيانات الرقم القومي؟
IF NOT EXISTS (SELECT 1 FROM Questions WHERE Id = 'd0e1f2a3-b4c5-6789-0123-def123456789')
BEGIN
    INSERT INTO Questions (Id, Title, Content, UserId, CategoryId, UpVotes, DownVotes, Views, AnswersCount, Score, IsClosed, IsDeleted, CreatedAt)
    VALUES ('d0e1f2a3-b4c5-6789-0123-def123456789', N'للاستعلام عن بيانات الرقم القومي؟', N'للاستعلام عن بيانات الرقم القومي؟', '00000000-0000-0000-0000-000000000001', '5E0B657A-5F9A-4E38-B5A1-987654321011', 0, 0, 0, 1, 0, 0, 0, GETUTCDATE());
    INSERT INTO Answers (Id, Content, QuestionId, UserId, IsAccepted, UpVotes, DownVotes, SuccessCount, Score, IsDeleted, CreatedAt)
    VALUES ('e1f2a3b4-c5d6-7890-1234-ef1234567890', N'للاستعلام عن بيانات الرقم القومي (مثل: الاسم الرباعي، تاريخ الميلاد، العنوان المسجل)، تقدر ترسل رسالة نصية من تليفونك المحمول إلى 9999 بالصيغة التالية: 'رقم قومي (ثم الرقم القومي)'. مثال: 'رقم قومي 12345678901234'. هتجيلك رسالة رد فيها الاسم وتاريخ الميلاد. الخدمة برسوم 3 جنيه. كمان تقدر تروح أي سجل مدني تطلب 'بيان بيانات' وبتدفع 10 جنيهات. (الموقع: القاهرة)', 'd0e1f2a3-b4c5-6789-0123-def123456789', '00000000-0000-0000-0000-000000000001', 0, 32, 0, 0, 64, 0, '2026-08-13');
END

-- 4. Map Tags to Questions
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'بطاقة رقم قومي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f47ac10b-58cc-4372-a567-0e02b2c3d479')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f47ac10b-58cc-4372-a567-0e02b2c3d479' AND TagId = (SELECT Id FROM Tags WHERE Name = N'بطاقة رقم قومي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f47ac10b-58cc-4372-a567-0e02b2c3d479', (SELECT Id FROM Tags WHERE Name = N'بطاقة رقم قومي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مستندات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f47ac10b-58cc-4372-a567-0e02b2c3d479')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f47ac10b-58cc-4372-a567-0e02b2c3d479' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مستندات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f47ac10b-58cc-4372-a567-0e02b2c3d479', (SELECT Id FROM Tags WHERE Name = N'مستندات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'أول مرة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f47ac10b-58cc-4372-a567-0e02b2c3d479')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f47ac10b-58cc-4372-a567-0e02b2c3d479' AND TagId = (SELECT Id FROM Tags WHERE Name = N'أول مرة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f47ac10b-58cc-4372-a567-0e02b2c3d479', (SELECT Id FROM Tags WHERE Name = N'أول مرة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تجديد بطاقة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = '7c9e6679-7425-40de-944b-e07fc1f90ae7')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = '7c9e6679-7425-40de-944b-e07fc1f90ae7' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تجديد بطاقة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('7c9e6679-7425-40de-944b-e07fc1f90ae7', (SELECT Id FROM Tags WHERE Name = N'تجديد بطاقة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'أوراق مطلوبة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = '7c9e6679-7425-40de-944b-e07fc1f90ae7')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = '7c9e6679-7425-40de-944b-e07fc1f90ae7' AND TagId = (SELECT Id FROM Tags WHERE Name = N'أوراق مطلوبة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('7c9e6679-7425-40de-944b-e07fc1f90ae7', (SELECT Id FROM Tags WHERE Name = N'أوراق مطلوبة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'عنوان ثابت') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'عنوان ثابت'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c', (SELECT Id FROM Tags WHERE Name = N'عنوان ثابت'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'إثبات عنوان') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'إثبات عنوان'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a3bb4e5b-9d4b-4a3e-8d5f-3b2c1a6e9d8c', (SELECT Id FROM Tags WHERE Name = N'إثبات عنوان'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مدة استخراج') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مدة استخراج'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a', (SELECT Id FROM Tags WHERE Name = N'مدة استخراج'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'وقت') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'وقت'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d4e5f6a7-b8c9-0d1e-2f3a-4b5c6d7e8f9a', (SELECT Id FROM Tags WHERE Name = N'وقت'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'خدمة مستعجلة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'خدمة مستعجلة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c', (SELECT Id FROM Tags WHERE Name = N'خدمة مستعجلة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'سرعة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'سرعة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f6a7b8c9-d0e1-2f3a-4b5c-6d7e8f9a0b1c', (SELECT Id FROM Tags WHERE Name = N'سرعة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'سجل مدني') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'سجل مدني'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'سجل مدني'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مولات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مولات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'مولات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'خدمات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'خدمات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'خدمات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تغيير مهنة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd0e1f2a3-b4c5-6789-0123-def123456789')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd0e1f2a3-b4c5-6789-0123-def123456789' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تغيير مهنة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d0e1f2a3-b4c5-6789-0123-def123456789', (SELECT Id FROM Tags WHERE Name = N'تغيير مهنة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تعديل بيانات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd0e1f2a3-b4c5-6789-0123-def123456789')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd0e1f2a3-b4c5-6789-0123-def123456789' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تعديل بيانات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d0e1f2a3-b4c5-6789-0123-def123456789', (SELECT Id FROM Tags WHERE Name = N'تعديل بيانات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مهنة حر') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مهنة حر'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c', (SELECT Id FROM Tags WHERE Name = N'مهنة حر'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'إثبات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'إثبات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c', (SELECT Id FROM Tags WHERE Name = N'إثبات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'شهادة مزاولة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'شهادة مزاولة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c', (SELECT Id FROM Tags WHERE Name = N'شهادة مزاولة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تغيير عنوان') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تغيير عنوان'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e', (SELECT Id FROM Tags WHERE Name = N'تغيير عنوان'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'عقد إيجار') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e' AND TagId = (SELECT Id FROM Tags WHERE Name = N'عقد إيجار'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e', (SELECT Id FROM Tags WHERE Name = N'عقد إيجار'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'توثيق') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e' AND TagId = (SELECT Id FROM Tags WHERE Name = N'توثيق'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e', (SELECT Id FROM Tags WHERE Name = N'توثيق'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تكلفة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تكلفة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a', (SELECT Id FROM Tags WHERE Name = N'تكلفة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'رسوم') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'رسوم'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a', (SELECT Id FROM Tags WHERE Name = N'رسوم'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'سعر') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'سعر'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d6e7f8a9-b0c1-2d3e-4f5a-6b7c8d9e0f1a', (SELECT Id FROM Tags WHERE Name = N'سعر'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'غرامة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'غرامة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c', (SELECT Id FROM Tags WHERE Name = N'غرامة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'انتهاء صلاحية') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'انتهاء صلاحية'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c', (SELECT Id FROM Tags WHERE Name = N'انتهاء صلاحية'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تأخير') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تأخير'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f8a9b0c1-d2e3-4f5a-6b7c-8d9e0f1a2b3c', (SELECT Id FROM Tags WHERE Name = N'تأخير'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'بدل فاقد') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e' AND TagId = (SELECT Id FROM Tags WHERE Name = N'بدل فاقد'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e', (SELECT Id FROM Tags WHERE Name = N'بدل فاقد'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'ضائع') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e' AND TagId = (SELECT Id FROM Tags WHERE Name = N'ضائع'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e', (SELECT Id FROM Tags WHERE Name = N'ضائع'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'إجراءات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e' AND TagId = (SELECT Id FROM Tags WHERE Name = N'إجراءات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e', (SELECT Id FROM Tags WHERE Name = N'إجراءات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'حضور شخصي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'حضور شخصي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a', (SELECT Id FROM Tags WHERE Name = N'حضور شخصي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'توكيل') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'توكيل'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a', (SELECT Id FROM Tags WHERE Name = N'توكيل'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'أفضل وقت') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'أفضل وقت'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c', (SELECT Id FROM Tags WHERE Name = N'أفضل وقت'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'زحمة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'زحمة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c', (SELECT Id FROM Tags WHERE Name = N'زحمة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مواعيد') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مواعيد'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c', (SELECT Id FROM Tags WHERE Name = N'مواعيد'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'العباسية') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b6c7d8e9-f0a1-2345-6789-0abcdef12345')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b6c7d8e9-f0a1-2345-6789-0abcdef12345' AND TagId = (SELECT Id FROM Tags WHERE Name = N'العباسية'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b6c7d8e9-f0a1-2345-6789-0abcdef12345', (SELECT Id FROM Tags WHERE Name = N'العباسية'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'زحمة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b6c7d8e9-f0a1-2345-6789-0abcdef12345')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b6c7d8e9-f0a1-2345-6789-0abcdef12345' AND TagId = (SELECT Id FROM Tags WHERE Name = N'زحمة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b6c7d8e9-f0a1-2345-6789-0abcdef12345', (SELECT Id FROM Tags WHERE Name = N'زحمة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مكتب القاهرة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b6c7d8e9-f0a1-2345-6789-0abcdef12345')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b6c7d8e9-f0a1-2345-6789-0abcdef12345' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مكتب القاهرة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b6c7d8e9-f0a1-2345-6789-0abcdef12345', (SELECT Id FROM Tags WHERE Name = N'مكتب القاهرة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'حالة اجتماعية') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'حالة اجتماعية'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a', (SELECT Id FROM Tags WHERE Name = N'حالة اجتماعية'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'متزوج') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'متزوج'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a', (SELECT Id FROM Tags WHERE Name = N'متزوج'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مطلق') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مطلق'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a', (SELECT Id FROM Tags WHERE Name = N'مطلق'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'أرمل') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a' AND TagId = (SELECT Id FROM Tags WHERE Name = N'أرمل'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a', (SELECT Id FROM Tags WHERE Name = N'أرمل'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'حضور الزوج') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f0a1b2c3-d4e5-6789-0123-f45678901234')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f0a1b2c3-d4e5-6789-0123-f45678901234' AND TagId = (SELECT Id FROM Tags WHERE Name = N'حضور الزوج'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f0a1b2c3-d4e5-6789-0123-f45678901234', (SELECT Id FROM Tags WHERE Name = N'حضور الزوج'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تغيير حالة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f0a1b2c3-d4e5-6789-0123-f45678901234')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f0a1b2c3-d4e5-6789-0123-f45678901234' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تغيير حالة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f0a1b2c3-d4e5-6789-0123-f45678901234', (SELECT Id FROM Tags WHERE Name = N'تغيير حالة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيد عائلي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a1b2c3d4-e5f6-7890-1234-567890abcdef' AND TagId = (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'إجراءات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a1b2c3d4-e5f6-7890-1234-567890abcdef' AND TagId = (SELECT Id FROM Tags WHERE Name = N'إجراءات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', (SELECT Id FROM Tags WHERE Name = N'إجراءات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مستندات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a1b2c3d4-e5f6-7890-1234-567890abcdef' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مستندات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', (SELECT Id FROM Tags WHERE Name = N'مستندات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مدة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'c3d4e5f6-a7b8-9012-3456-7890abcdef12')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'c3d4e5f6-a7b8-9012-3456-7890abcdef12' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مدة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('c3d4e5f6-a7b8-9012-3456-7890abcdef12', (SELECT Id FROM Tags WHERE Name = N'مدة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيد عائلي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'c3d4e5f6-a7b8-9012-3456-7890abcdef12')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'c3d4e5f6-a7b8-9012-3456-7890abcdef12' AND TagId = (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('c3d4e5f6-a7b8-9012-3456-7890abcdef12', (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'وقت') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'c3d4e5f6-a7b8-9012-3456-7890abcdef12')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'c3d4e5f6-a7b8-9012-3456-7890abcdef12' AND TagId = (SELECT Id FROM Tags WHERE Name = N'وقت'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('c3d4e5f6-a7b8-9012-3456-7890abcdef12', (SELECT Id FROM Tags WHERE Name = N'وقت'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيد فردي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e5f6a7b8-c9d0-1234-5678-90abcdef1234')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e5f6a7b8-c9d0-1234-5678-90abcdef1234' AND TagId = (SELECT Id FROM Tags WHERE Name = N'قيد فردي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e5f6a7b8-c9d0-1234-5678-90abcdef1234', (SELECT Id FROM Tags WHERE Name = N'قيد فردي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيد عائلي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e5f6a7b8-c9d0-1234-5678-90abcdef1234')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e5f6a7b8-c9d0-1234-5678-90abcdef1234' AND TagId = (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e5f6a7b8-c9d0-1234-5678-90abcdef1234', (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'فرق') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e5f6a7b8-c9d0-1234-5678-90abcdef1234')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e5f6a7b8-c9d0-1234-5678-90abcdef1234' AND TagId = (SELECT Id FROM Tags WHERE Name = N'فرق'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e5f6a7b8-c9d0-1234-5678-90abcdef1234', (SELECT Id FROM Tags WHERE Name = N'فرق'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'أونلاين') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a7b8c9d0-e1f2-3456-7890-abcdef123456')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a7b8c9d0-e1f2-3456-7890-abcdef123456' AND TagId = (SELECT Id FROM Tags WHERE Name = N'أونلاين'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a7b8c9d0-e1f2-3456-7890-abcdef123456', (SELECT Id FROM Tags WHERE Name = N'أونلاين'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيد عائلي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a7b8c9d0-e1f2-3456-7890-abcdef123456')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a7b8c9d0-e1f2-3456-7890-abcdef123456' AND TagId = (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a7b8c9d0-e1f2-3456-7890-abcdef123456', (SELECT Id FROM Tags WHERE Name = N'قيد عائلي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'بوابة حكومية') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a7b8c9d0-e1f2-3456-7890-abcdef123456')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a7b8c9d0-e1f2-3456-7890-abcdef123456' AND TagId = (SELECT Id FROM Tags WHERE Name = N'بوابة حكومية'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a7b8c9d0-e1f2-3456-7890-abcdef123456', (SELECT Id FROM Tags WHERE Name = N'بوابة حكومية'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'رخصة قيادة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e9f0a1b2-c3d4-5678-9012-ef4567890123' AND TagId = (SELECT Id FROM Tags WHERE Name = N'رخصة قيادة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', (SELECT Id FROM Tags WHERE Name = N'رخصة قيادة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'أول مرة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e9f0a1b2-c3d4-5678-9012-ef4567890123' AND TagId = (SELECT Id FROM Tags WHERE Name = N'أول مرة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', (SELECT Id FROM Tags WHERE Name = N'أول مرة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'إجراءات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e9f0a1b2-c3d4-5678-9012-ef4567890123' AND TagId = (SELECT Id FROM Tags WHERE Name = N'إجراءات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', (SELECT Id FROM Tags WHERE Name = N'إجراءات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تجديد رخصة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a1b2c3d4-e5f6-7890-1234-567890abcdef' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تجديد رخصة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', (SELECT Id FROM Tags WHERE Name = N'تجديد رخصة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'أوراق مطلوبة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a1b2c3d4-e5f6-7890-1234-567890abcdef' AND TagId = (SELECT Id FROM Tags WHERE Name = N'أوراق مطلوبة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', (SELECT Id FROM Tags WHERE Name = N'أوراق مطلوبة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'قيادة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'a1b2c3d4-e5f6-7890-1234-567890abcdef' AND TagId = (SELECT Id FROM Tags WHERE Name = N'قيادة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', (SELECT Id FROM Tags WHERE Name = N'قيادة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تكلفة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تكلفة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'تكلفة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'رخصة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'رخصة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'رخصة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'رسوم') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'رسوم'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'رسوم'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مخالفات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4567-8901-f78901234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f8a9b0c1-d2e3-4567-8901-f78901234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مخالفات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f8a9b0c1-d2e3-4567-8901-f78901234567', (SELECT Id FROM Tags WHERE Name = N'مخالفات'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'استعلام') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4567-8901-f78901234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f8a9b0c1-d2e3-4567-8901-f78901234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'استعلام'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f8a9b0c1-d2e3-4567-8901-f78901234567', (SELECT Id FROM Tags WHERE Name = N'استعلام'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مرور') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'f8a9b0c1-d2e3-4567-8901-f78901234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'f8a9b0c1-d2e3-4567-8901-f78901234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مرور'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('f8a9b0c1-d2e3-4567-8901-f78901234567', (SELECT Id FROM Tags WHERE Name = N'مرور'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'بيان نجاح') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'c7d8e9f0-a1b2-3456-7890-cdef45678901')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'c7d8e9f0-a1b2-3456-7890-cdef45678901' AND TagId = (SELECT Id FROM Tags WHERE Name = N'بيان نجاح'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('c7d8e9f0-a1b2-3456-7890-cdef45678901', (SELECT Id FROM Tags WHERE Name = N'بيان نجاح'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'شهادة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'c7d8e9f0-a1b2-3456-7890-cdef45678901')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'c7d8e9f0-a1b2-3456-7890-cdef45678901' AND TagId = (SELECT Id FROM Tags WHERE Name = N'شهادة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('c7d8e9f0-a1b2-3456-7890-cdef45678901', (SELECT Id FROM Tags WHERE Name = N'شهادة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'مدرسة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'c7d8e9f0-a1b2-3456-7890-cdef45678901')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'c7d8e9f0-a1b2-3456-7890-cdef45678901' AND TagId = (SELECT Id FROM Tags WHERE Name = N'مدرسة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('c7d8e9f0-a1b2-3456-7890-cdef45678901', (SELECT Id FROM Tags WHERE Name = N'مدرسة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'شهادة تخرج') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e9f0a1b2-c3d4-5678-9012-ef4567890123' AND TagId = (SELECT Id FROM Tags WHERE Name = N'شهادة تخرج'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', (SELECT Id FROM Tags WHERE Name = N'شهادة تخرج'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'بدل فاقد') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e9f0a1b2-c3d4-5678-9012-ef4567890123' AND TagId = (SELECT Id FROM Tags WHERE Name = N'بدل فاقد'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', (SELECT Id FROM Tags WHERE Name = N'بدل فاقد'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'جامعة') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'e9f0a1b2-c3d4-5678-9012-ef4567890123')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'e9f0a1b2-c3d4-5678-9012-ef4567890123' AND TagId = (SELECT Id FROM Tags WHERE Name = N'جامعة'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('e9f0a1b2-c3d4-5678-9012-ef4567890123', (SELECT Id FROM Tags WHERE Name = N'جامعة'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'سجل تجاري') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'سجل تجاري'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'سجل تجاري'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'تراخيص') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'تراخيص'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'تراخيص'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'محل') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'b8c9d0e1-f2a3-4567-8901-bcdef1234567' AND TagId = (SELECT Id FROM Tags WHERE Name = N'محل'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('b8c9d0e1-f2a3-4567-8901-bcdef1234567', (SELECT Id FROM Tags WHERE Name = N'محل'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'استعلام') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd0e1f2a3-b4c5-6789-0123-def123456789')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd0e1f2a3-b4c5-6789-0123-def123456789' AND TagId = (SELECT Id FROM Tags WHERE Name = N'استعلام'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d0e1f2a3-b4c5-6789-0123-def123456789', (SELECT Id FROM Tags WHERE Name = N'استعلام'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'رقم قومي') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd0e1f2a3-b4c5-6789-0123-def123456789')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd0e1f2a3-b4c5-6789-0123-def123456789' AND TagId = (SELECT Id FROM Tags WHERE Name = N'رقم قومي'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d0e1f2a3-b4c5-6789-0123-def123456789', (SELECT Id FROM Tags WHERE Name = N'رقم قومي'));
IF EXISTS (SELECT 1 FROM Tags WHERE Name = N'بيانات') AND EXISTS (SELECT 1 FROM Questions WHERE Id = 'd0e1f2a3-b4c5-6789-0123-def123456789')
    IF NOT EXISTS (SELECT 1 FROM QuestionTags WHERE QuestionId = 'd0e1f2a3-b4c5-6789-0123-def123456789' AND TagId = (SELECT Id FROM Tags WHERE Name = N'بيانات'))
        INSERT INTO QuestionTags (QuestionId, TagId) VALUES ('d0e1f2a3-b4c5-6789-0123-def123456789', (SELECT Id FROM Tags WHERE Name = N'بيانات'));

-- 5. Update Counts
UPDATE Categories SET QuestionsCount = (SELECT COUNT(*) FROM Questions WHERE CategoryId = Categories.Id AND IsDeleted = 0);
UPDATE Tags SET UsageCount = (SELECT COUNT(*) FROM QuestionTags WHERE TagId = Tags.Id);