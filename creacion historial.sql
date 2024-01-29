select * from treatments
select * from Sufferings
select * from medicines

select * from MedicalHistorySufferings
select * from MedicalHistoryMedicines
select * from MedicalHistoryTreatments
select * from medicalimages
select * from medicalnotes

select * from physicians

select * from MedicalHistories


insert into medicalhistorySufferings values ('e99ef76e-3a75-496f-839b-916ee48a7040', 1, 1)

insert into medicalhistorytreatments values ('e99ef76e-3a75-496f-839b-916ee48a7040', 1, 1)
insert into medicalhistorytreatments values ('e99ef76e-3a75-496f-839b-916ee48a7040', 2, 1)

insert into medicalhistoryMedicines values ('e99ef76e-3a75-496f-839b-916ee48a7040', 1, 1)


insert into medicalnotes values ('note1', GETDATE(), getdate(), 1, 'e99ef76e-3a75-496f-839b-916ee48a7040')

insert into medicalimages values ('/images/url', 'pdfs/url', 'e99ef76e-3a75-496f-839b-916ee48a7040', 'como es que es')


select * from medicalhistory