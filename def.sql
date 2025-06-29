PGDMP  !                    }            private_clinic    17.5    17.5 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            �           1262    17077    private_clinic    DATABASE     �   CREATE DATABASE private_clinic WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = icu LOCALE = 'C.UTF-8' ICU_LOCALE = 'ru-RU';
    DROP DATABASE private_clinic;
                     postgres    false            �            1259    17290    appointment_diagnosis    TABLE     p   CREATE TABLE public.appointment_diagnosis (
    appointment integer NOT NULL,
    diagnosis integer NOT NULL
);
 )   DROP TABLE public.appointment_diagnosis;
       public         heap r       postgres    false            �            1259    17078    appointment_equipments    TABLE     q   CREATE TABLE public.appointment_equipments (
    appointment integer NOT NULL,
    equipment integer NOT NULL
);
 *   DROP TABLE public.appointment_equipments;
       public         heap r       postgres    false            �            1259    17081    appointment_services    TABLE     m   CREATE TABLE public.appointment_services (
    appointment integer NOT NULL,
    service integer NOT NULL
);
 (   DROP TABLE public.appointment_services;
       public         heap r       postgres    false            �            1259    17084    appointments    TABLE     s  CREATE TABLE public.appointments (
    appointment_number integer NOT NULL,
    date date NOT NULL,
    patient integer NOT NULL,
    purpose integer NOT NULL,
    office integer NOT NULL,
    total_price money NOT NULL,
    discount smallint,
    commentaries text,
    "isPlanned" boolean NOT NULL,
    specialist integer NOT NULL,
    "time" time without time zone
);
     DROP TABLE public.appointments;
       public         heap r       postgres    false            �            1259    17089    appointments_id_seq    SEQUENCE     |   CREATE SEQUENCE public.appointments_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.appointments_id_seq;
       public               postgres    false    219            �           0    0    appointments_id_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public.appointments_id_seq OWNED BY public.appointments.appointment_number;
          public               postgres    false    220            �            1259    17090    diagnosis_id_seq    SEQUENCE     y   CREATE SEQUENCE public.diagnosis_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.diagnosis_id_seq;
       public               postgres    false            �            1259    17091 	   diagnosis    TABLE     �   CREATE TABLE public.diagnosis (
    diagnosis_id integer DEFAULT nextval('public.diagnosis_id_seq'::regclass) NOT NULL,
    diagnosis_name text,
    description text NOT NULL
);
    DROP TABLE public.diagnosis;
       public         heap r       postgres    false    221            �            1259    17097    equipments_id_seq    SEQUENCE     z   CREATE SEQUENCE public.equipments_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.equipments_id_seq;
       public               postgres    false            �            1259    17098 
   equipments    TABLE     �   CREATE TABLE public.equipments (
    equipment_id integer DEFAULT nextval('public.equipments_id_seq'::regclass) NOT NULL,
    equipment_name text,
    expiration_date date NOT NULL,
    supplier integer,
    description text NOT NULL
);
    DROP TABLE public.equipments;
       public         heap r       postgres    false    223            �            1259    18658    medical_checkup_appointments    TABLE     �   CREATE TABLE public.medical_checkup_appointments (
    checkup_plan_id integer NOT NULL,
    appointment_id integer NOT NULL
);
 0   DROP TABLE public.medical_checkup_appointments;
       public         heap r       postgres    false            �            1259    18639    medical_checkup_plans    TABLE       CREATE TABLE public.medical_checkup_plans (
    medical_checkup_id integer NOT NULL,
    patient_id integer NOT NULL,
    start_date date NOT NULL,
    end_date date,
    is_completed boolean DEFAULT false NOT NULL,
    checkup_type integer,
    comment text
);
 )   DROP TABLE public.medical_checkup_plans;
       public         heap r       postgres    false            �            1259    18638 ,   medical_checkup_plans_medical_checkup_id_seq    SEQUENCE     �   CREATE SEQUENCE public.medical_checkup_plans_medical_checkup_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 C   DROP SEQUENCE public.medical_checkup_plans_medical_checkup_id_seq;
       public               postgres    false    248            �           0    0 ,   medical_checkup_plans_medical_checkup_id_seq    SEQUENCE OWNED BY     }   ALTER SEQUENCE public.medical_checkup_plans_medical_checkup_id_seq OWNED BY public.medical_checkup_plans.medical_checkup_id;
          public               postgres    false    247            �            1259    18630    medical_checkup_types    TABLE     �   CREATE TABLE public.medical_checkup_types (
    medical_checkup_type_id integer NOT NULL,
    medical_checkup_name text NOT NULL,
    description text NOT NULL
);
 )   DROP TABLE public.medical_checkup_types;
       public         heap r       postgres    false            �            1259    18629 1   medical_checkup_types_medical_checkup_type_id_seq    SEQUENCE     �   CREATE SEQUENCE public.medical_checkup_types_medical_checkup_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 H   DROP SEQUENCE public.medical_checkup_types_medical_checkup_type_id_seq;
       public               postgres    false    246            �           0    0 1   medical_checkup_types_medical_checkup_type_id_seq    SEQUENCE OWNED BY     �   ALTER SEQUENCE public.medical_checkup_types_medical_checkup_type_id_seq OWNED BY public.medical_checkup_types.medical_checkup_type_id;
          public               postgres    false    245            �            1259    17104    office_equipment    TABLE     f   CREATE TABLE public.office_equipment (
    office integer NOT NULL,
    equipment integer NOT NULL
);
 $   DROP TABLE public.office_equipment;
       public         heap r       postgres    false            �            1259    17107    office_types_id_seq    SEQUENCE     |   CREATE SEQUENCE public.office_types_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.office_types_id_seq;
       public               postgres    false            �            1259    17108    office_types    TABLE     �   CREATE TABLE public.office_types (
    type_id integer DEFAULT nextval('public.office_types_id_seq'::regclass) NOT NULL,
    type_name text,
    description text NOT NULL
);
     DROP TABLE public.office_types;
       public         heap r       postgres    false    226            �            1259    17114    offices_id_seq    SEQUENCE     w   CREATE SEQUENCE public.offices_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.offices_id_seq;
       public               postgres    false            �            1259    17115    offices    TABLE     �   CREATE TABLE public.offices (
    office_id integer DEFAULT nextval('public.offices_id_seq'::regclass) NOT NULL,
    number text,
    type integer NOT NULL
);
    DROP TABLE public.offices;
       public         heap r       postgres    false    228            �            1259    17121    patients    TABLE     �  CREATE TABLE public.patients (
    patient_id integer NOT NULL,
    first_name character varying(255) NOT NULL,
    middle_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    insurance_id numeric(8,0) DEFAULT 0 NOT NULL,
    birthday date NOT NULL,
    gender character(1) NOT NULL,
    phone numeric(11,0) NOT NULL,
    address text NOT NULL,
    CONSTRAINT gender_check CHECK ((gender = ANY (ARRAY['f'::bpchar, 'm'::bpchar])))
);
    DROP TABLE public.patients;
       public         heap r       postgres    false            �            1259    17128    patients_patient_id_seq    SEQUENCE     �   CREATE SEQUENCE public.patients_patient_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public.patients_patient_id_seq;
       public               postgres    false    230            �           0    0    patients_patient_id_seq    SEQUENCE OWNED BY     S   ALTER SEQUENCE public.patients_patient_id_seq OWNED BY public.patients.patient_id;
          public               postgres    false    231            �            1259    17129    purposes    TABLE     Y   CREATE TABLE public.purposes (
    purpose_id integer NOT NULL,
    purpose_name text
);
    DROP TABLE public.purposes;
       public         heap r       postgres    false            �            1259    17134    purposes_id_seq    SEQUENCE     x   CREATE SEQUENCE public.purposes_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.purposes_id_seq;
       public               postgres    false            �            1259    17135    receipts    TABLE     �   CREATE TABLE public.receipts (
    receipt_id integer NOT NULL,
    appointment_id integer NOT NULL,
    specialist_id integer NOT NULL,
    total_summary money NOT NULL,
    date date NOT NULL
);
    DROP TABLE public.receipts;
       public         heap r       postgres    false            �            1259    17138    receipts_receipt_id_seq    SEQUENCE     �   CREATE SEQUENCE public.receipts_receipt_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public.receipts_receipt_id_seq;
       public               postgres    false    234            �           0    0    receipts_receipt_id_seq    SEQUENCE OWNED BY     S   ALTER SEQUENCE public.receipts_receipt_id_seq OWNED BY public.receipts.receipt_id;
          public               postgres    false    235            �            1259    17139    services    TABLE     �   CREATE TABLE public.services (
    id integer NOT NULL,
    name text NOT NULL,
    specialist integer NOT NULL,
    description text DEFAULT 'no_description'::text NOT NULL,
    price money NOT NULL
);
    DROP TABLE public.services;
       public         heap r       postgres    false            �            1259    17145    services_id_seq    SEQUENCE     �   CREATE SEQUENCE public.services_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.services_id_seq;
       public               postgres    false    236            �           0    0    services_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.services_id_seq OWNED BY public.services.id;
          public               postgres    false    237            �            1259    17146    specialists    TABLE     �  CREATE TABLE public.specialists (
    id integer NOT NULL,
    first_name character varying(255) NOT NULL,
    middle_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    specialization integer NOT NULL,
    specialization_type text NOT NULL,
    phone numeric(11,0) NOT NULL,
    CONSTRAINT specialization_type CHECK ((specialization_type = ANY (ARRAY[('standard'::character varying)::text, ('leading'::character varying)::text])))
);
    DROP TABLE public.specialists;
       public         heap r       postgres    false            �            1259    17152    specialists_id_seq    SEQUENCE     �   CREATE SEQUENCE public.specialists_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public.specialists_id_seq;
       public               postgres    false    238            �           0    0    specialists_id_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public.specialists_id_seq OWNED BY public.specialists.id;
          public               postgres    false    239            �            1259    17153    specializations    TABLE     t   CREATE TABLE public.specializations (
    id integer NOT NULL,
    specialization character varying(80) NOT NULL
);
 #   DROP TABLE public.specializations;
       public         heap r       postgres    false            �            1259    17156    specializations_id_seq    SEQUENCE     �   CREATE SEQUENCE public.specializations_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.specializations_id_seq;
       public               postgres    false    240            �           0    0    specializations_id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public.specializations_id_seq OWNED BY public.specializations.id;
          public               postgres    false    241            �            1259    17157    users    TABLE     �   CREATE TABLE public.users (
    user_id integer NOT NULL,
    user_name text NOT NULL,
    password_hash text NOT NULL,
    salt text NOT NULL,
    is_active boolean DEFAULT false NOT NULL,
    is_superuser boolean DEFAULT false NOT NULL
);
    DROP TABLE public.users;
       public         heap r       postgres    false            �            1259    17163    users_user_id_seq    SEQUENCE     �   CREATE SEQUENCE public.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.users_user_id_seq;
       public               postgres    false    242            �           0    0    users_user_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;
          public               postgres    false    243            �           2604    17164    appointments appointment_number    DEFAULT     �   ALTER TABLE ONLY public.appointments ALTER COLUMN appointment_number SET DEFAULT nextval('public.appointments_id_seq'::regclass);
 N   ALTER TABLE public.appointments ALTER COLUMN appointment_number DROP DEFAULT;
       public               postgres    false    220    219            �           2604    18642 (   medical_checkup_plans medical_checkup_id    DEFAULT     �   ALTER TABLE ONLY public.medical_checkup_plans ALTER COLUMN medical_checkup_id SET DEFAULT nextval('public.medical_checkup_plans_medical_checkup_id_seq'::regclass);
 W   ALTER TABLE public.medical_checkup_plans ALTER COLUMN medical_checkup_id DROP DEFAULT;
       public               postgres    false    247    248    248            �           2604    18633 -   medical_checkup_types medical_checkup_type_id    DEFAULT     �   ALTER TABLE ONLY public.medical_checkup_types ALTER COLUMN medical_checkup_type_id SET DEFAULT nextval('public.medical_checkup_types_medical_checkup_type_id_seq'::regclass);
 \   ALTER TABLE public.medical_checkup_types ALTER COLUMN medical_checkup_type_id DROP DEFAULT;
       public               postgres    false    246    245    246            �           2604    17165    patients patient_id    DEFAULT     z   ALTER TABLE ONLY public.patients ALTER COLUMN patient_id SET DEFAULT nextval('public.patients_patient_id_seq'::regclass);
 B   ALTER TABLE public.patients ALTER COLUMN patient_id DROP DEFAULT;
       public               postgres    false    231    230            �           2604    17166    receipts receipt_id    DEFAULT     z   ALTER TABLE ONLY public.receipts ALTER COLUMN receipt_id SET DEFAULT nextval('public.receipts_receipt_id_seq'::regclass);
 B   ALTER TABLE public.receipts ALTER COLUMN receipt_id DROP DEFAULT;
       public               postgres    false    235    234            �           2604    17167    services id    DEFAULT     j   ALTER TABLE ONLY public.services ALTER COLUMN id SET DEFAULT nextval('public.services_id_seq'::regclass);
 :   ALTER TABLE public.services ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    237    236            �           2604    17168    specialists id    DEFAULT     p   ALTER TABLE ONLY public.specialists ALTER COLUMN id SET DEFAULT nextval('public.specialists_id_seq'::regclass);
 =   ALTER TABLE public.specialists ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    239    238            �           2604    17169    specializations id    DEFAULT     x   ALTER TABLE ONLY public.specializations ALTER COLUMN id SET DEFAULT nextval('public.specializations_id_seq'::regclass);
 A   ALTER TABLE public.specializations ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    241    240            �           2604    17170    users user_id    DEFAULT     n   ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);
 <   ALTER TABLE public.users ALTER COLUMN user_id DROP DEFAULT;
       public               postgres    false    243    242            �          0    17290    appointment_diagnosis 
   TABLE DATA           G   COPY public.appointment_diagnosis (appointment, diagnosis) FROM stdin;
    public               postgres    false    244   ��       �          0    17078    appointment_equipments 
   TABLE DATA           H   COPY public.appointment_equipments (appointment, equipment) FROM stdin;
    public               postgres    false    217   �       �          0    17081    appointment_services 
   TABLE DATA           D   COPY public.appointment_services (appointment, service) FROM stdin;
    public               postgres    false    218   z�       �          0    17084    appointments 
   TABLE DATA           �   COPY public.appointments (appointment_number, date, patient, purpose, office, total_price, discount, commentaries, "isPlanned", specialist, "time") FROM stdin;
    public               postgres    false    219   �       �          0    17091 	   diagnosis 
   TABLE DATA           N   COPY public.diagnosis (diagnosis_id, diagnosis_name, description) FROM stdin;
    public               postgres    false    222   ��       �          0    17098 
   equipments 
   TABLE DATA           j   COPY public.equipments (equipment_id, equipment_name, expiration_date, supplier, description) FROM stdin;
    public               postgres    false    224   ��       �          0    18658    medical_checkup_appointments 
   TABLE DATA           W   COPY public.medical_checkup_appointments (checkup_plan_id, appointment_id) FROM stdin;
    public               postgres    false    249   ��       �          0    18639    medical_checkup_plans 
   TABLE DATA           �   COPY public.medical_checkup_plans (medical_checkup_id, patient_id, start_date, end_date, is_completed, checkup_type, comment) FROM stdin;
    public               postgres    false    248   �       �          0    18630    medical_checkup_types 
   TABLE DATA           k   COPY public.medical_checkup_types (medical_checkup_type_id, medical_checkup_name, description) FROM stdin;
    public               postgres    false    246   0�       �          0    17104    office_equipment 
   TABLE DATA           =   COPY public.office_equipment (office, equipment) FROM stdin;
    public               postgres    false    225   M�       �          0    17108    office_types 
   TABLE DATA           G   COPY public.office_types (type_id, type_name, description) FROM stdin;
    public               postgres    false    227   ��       �          0    17115    offices 
   TABLE DATA           :   COPY public.offices (office_id, number, type) FROM stdin;
    public               postgres    false    229   ��       �          0    17121    patients 
   TABLE DATA           �   COPY public.patients (patient_id, first_name, middle_name, last_name, insurance_id, birthday, gender, phone, address) FROM stdin;
    public               postgres    false    230   �       �          0    17129    purposes 
   TABLE DATA           <   COPY public.purposes (purpose_id, purpose_name) FROM stdin;
    public               postgres    false    232   [�       �          0    17135    receipts 
   TABLE DATA           b   COPY public.receipts (receipt_id, appointment_id, specialist_id, total_summary, date) FROM stdin;
    public               postgres    false    234   ��       �          0    17139    services 
   TABLE DATA           L   COPY public.services (id, name, specialist, description, price) FROM stdin;
    public               postgres    false    236   ��       �          0    17146    specialists 
   TABLE DATA           y   COPY public.specialists (id, first_name, middle_name, last_name, specialization, specialization_type, phone) FROM stdin;
    public               postgres    false    238   �      �          0    17153    specializations 
   TABLE DATA           =   COPY public.specializations (id, specialization) FROM stdin;
    public               postgres    false    240   (      �          0    17157    users 
   TABLE DATA           a   COPY public.users (user_id, user_name, password_hash, salt, is_active, is_superuser) FROM stdin;
    public               postgres    false    242   G+      �           0    0    appointments_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.appointments_id_seq', 98, true);
          public               postgres    false    220            �           0    0    diagnosis_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.diagnosis_id_seq', 50, true);
          public               postgres    false    221            �           0    0    equipments_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public.equipments_id_seq', 50, true);
          public               postgres    false    223            �           0    0 ,   medical_checkup_plans_medical_checkup_id_seq    SEQUENCE SET     [   SELECT pg_catalog.setval('public.medical_checkup_plans_medical_checkup_id_seq', 1, false);
          public               postgres    false    247            �           0    0 1   medical_checkup_types_medical_checkup_type_id_seq    SEQUENCE SET     `   SELECT pg_catalog.setval('public.medical_checkup_types_medical_checkup_type_id_seq', 1, false);
          public               postgres    false    245            �           0    0    office_types_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.office_types_id_seq', 24, true);
          public               postgres    false    226            �           0    0    offices_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.offices_id_seq', 24, true);
          public               postgres    false    228            �           0    0    patients_patient_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.patients_patient_id_seq', 51, true);
          public               postgres    false    231            �           0    0    purposes_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.purposes_id_seq', 100, true);
          public               postgres    false    233            �           0    0    receipts_receipt_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.receipts_receipt_id_seq', 31, true);
          public               postgres    false    235            �           0    0    services_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.services_id_seq', 193, true);
          public               postgres    false    237            �           0    0    specialists_id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public.specialists_id_seq', 61, true);
          public               postgres    false    239            �           0    0    specializations_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.specializations_id_seq', 75, true);
          public               postgres    false    241            �           0    0    users_user_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.users_user_id_seq', 6, true);
          public               postgres    false    243            �           2606    17172 2   appointment_equipments appointment_equipments_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public.appointment_equipments
    ADD CONSTRAINT appointment_equipments_pkey PRIMARY KEY (appointment, equipment) INCLUDE (appointment, equipment);
 \   ALTER TABLE ONLY public.appointment_equipments DROP CONSTRAINT appointment_equipments_pkey;
       public                 postgres    false    217    217            �           2606    17174    appointments appointments_pkey 
   CONSTRAINT     l   ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_pkey PRIMARY KEY (appointment_number);
 H   ALTER TABLE ONLY public.appointments DROP CONSTRAINT appointments_pkey;
       public                 postgres    false    219            �           2606    17176    diagnosis diagnosis_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.diagnosis
    ADD CONSTRAINT diagnosis_pkey PRIMARY KEY (diagnosis_id);
 B   ALTER TABLE ONLY public.diagnosis DROP CONSTRAINT diagnosis_pkey;
       public                 postgres    false    222            �           2606    17178    equipments equipments_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.equipments
    ADD CONSTRAINT equipments_pkey PRIMARY KEY (equipment_id);
 D   ALTER TABLE ONLY public.equipments DROP CONSTRAINT equipments_pkey;
       public                 postgres    false    224            �           2606    18662 >   medical_checkup_appointments medical_checkup_appointments_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public.medical_checkup_appointments
    ADD CONSTRAINT medical_checkup_appointments_pkey PRIMARY KEY (checkup_plan_id, appointment_id);
 h   ALTER TABLE ONLY public.medical_checkup_appointments DROP CONSTRAINT medical_checkup_appointments_pkey;
       public                 postgres    false    249    249            �           2606    18647 0   medical_checkup_plans medical_checkup_plans_pkey 
   CONSTRAINT     ~   ALTER TABLE ONLY public.medical_checkup_plans
    ADD CONSTRAINT medical_checkup_plans_pkey PRIMARY KEY (medical_checkup_id);
 Z   ALTER TABLE ONLY public.medical_checkup_plans DROP CONSTRAINT medical_checkup_plans_pkey;
       public                 postgres    false    248            �           2606    18637 0   medical_checkup_types medical_checkup_types_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public.medical_checkup_types
    ADD CONSTRAINT medical_checkup_types_pkey PRIMARY KEY (medical_checkup_type_id);
 Z   ALTER TABLE ONLY public.medical_checkup_types DROP CONSTRAINT medical_checkup_types_pkey;
       public                 postgres    false    246            �           2606    17180    offices offices_pkey 
   CONSTRAINT     Y   ALTER TABLE ONLY public.offices
    ADD CONSTRAINT offices_pkey PRIMARY KEY (office_id);
 >   ALTER TABLE ONLY public.offices DROP CONSTRAINT offices_pkey;
       public                 postgres    false    229            �           2606    17182    patients patients_pkey 
   CONSTRAINT     q   ALTER TABLE ONLY public.patients
    ADD CONSTRAINT patients_pkey PRIMARY KEY (patient_id) INCLUDE (patient_id);
 @   ALTER TABLE ONLY public.patients DROP CONSTRAINT patients_pkey;
       public                 postgres    false    230            �           2606    17184    appointment_services pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public.appointment_services
    ADD CONSTRAINT pkey PRIMARY KEY (appointment, service) INCLUDE (service, appointment);
 C   ALTER TABLE ONLY public.appointment_services DROP CONSTRAINT pkey;
       public                 postgres    false    218    218            �           2606    17294 0   appointment_diagnosis pkey_appointment_diagnosis 
   CONSTRAINT     �   ALTER TABLE ONLY public.appointment_diagnosis
    ADD CONSTRAINT pkey_appointment_diagnosis PRIMARY KEY (appointment, diagnosis);
 Z   ALTER TABLE ONLY public.appointment_diagnosis DROP CONSTRAINT pkey_appointment_diagnosis;
       public                 postgres    false    244    244            �           2606    17186 &   office_equipment pkey_office_equipment 
   CONSTRAINT     s   ALTER TABLE ONLY public.office_equipment
    ADD CONSTRAINT pkey_office_equipment PRIMARY KEY (office, equipment);
 P   ALTER TABLE ONLY public.office_equipment DROP CONSTRAINT pkey_office_equipment;
       public                 postgres    false    225    225            �           2606    17188    receipts pkey_receipt 
   CONSTRAINT     [   ALTER TABLE ONLY public.receipts
    ADD CONSTRAINT pkey_receipt PRIMARY KEY (receipt_id);
 ?   ALTER TABLE ONLY public.receipts DROP CONSTRAINT pkey_receipt;
       public                 postgres    false    234            �           2606    17190    purposes purposes_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.purposes
    ADD CONSTRAINT purposes_pkey PRIMARY KEY (purpose_id);
 @   ALTER TABLE ONLY public.purposes DROP CONSTRAINT purposes_pkey;
       public                 postgres    false    232            �           2606    17192    services services_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.services DROP CONSTRAINT services_pkey;
       public                 postgres    false    236            �           2606    17194    specialists specialists_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.specialists
    ADD CONSTRAINT specialists_pkey PRIMARY KEY (id);
 F   ALTER TABLE ONLY public.specialists DROP CONSTRAINT specialists_pkey;
       public                 postgres    false    238            �           2606    17196 #   specializations specialization_pkey 
   CONSTRAINT     a   ALTER TABLE ONLY public.specializations
    ADD CONSTRAINT specialization_pkey PRIMARY KEY (id);
 M   ALTER TABLE ONLY public.specializations DROP CONSTRAINT specialization_pkey;
       public                 postgres    false    240            �           2606    17198    office_types types_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.office_types
    ADD CONSTRAINT types_pkey PRIMARY KEY (type_id);
 A   ALTER TABLE ONLY public.office_types DROP CONSTRAINT types_pkey;
       public                 postgres    false    227            �           2606    17200    users users_pkey 
   CONSTRAINT     S   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public                 postgres    false    242                       2606    18668 +   medical_checkup_appointments fk_appointment    FK CONSTRAINT     �   ALTER TABLE ONLY public.medical_checkup_appointments
    ADD CONSTRAINT fk_appointment FOREIGN KEY (appointment_id) REFERENCES public.appointments(appointment_number) ON DELETE CASCADE;
 U   ALTER TABLE ONLY public.medical_checkup_appointments DROP CONSTRAINT fk_appointment;
       public               postgres    false    219    3295    249                       2606    18663 ,   medical_checkup_appointments fk_checkup_plan    FK CONSTRAINT     �   ALTER TABLE ONLY public.medical_checkup_appointments
    ADD CONSTRAINT fk_checkup_plan FOREIGN KEY (checkup_plan_id) REFERENCES public.medical_checkup_plans(medical_checkup_id) ON DELETE CASCADE;
 V   ALTER TABLE ONLY public.medical_checkup_appointments DROP CONSTRAINT fk_checkup_plan;
       public               postgres    false    3325    249    248                       2606    18653 %   medical_checkup_plans fk_checkup_type    FK CONSTRAINT     �   ALTER TABLE ONLY public.medical_checkup_plans
    ADD CONSTRAINT fk_checkup_type FOREIGN KEY (checkup_type) REFERENCES public.medical_checkup_types(medical_checkup_type_id);
 O   ALTER TABLE ONLY public.medical_checkup_plans DROP CONSTRAINT fk_checkup_type;
       public               postgres    false    246    3323    248                       2606    18648     medical_checkup_plans fk_patient    FK CONSTRAINT     �   ALTER TABLE ONLY public.medical_checkup_plans
    ADD CONSTRAINT fk_patient FOREIGN KEY (patient_id) REFERENCES public.patients(patient_id);
 J   ALTER TABLE ONLY public.medical_checkup_plans DROP CONSTRAINT fk_patient;
       public               postgres    false    230    3307    248            
           2606    17201    offices fkey _room_type    FK CONSTRAINT     �   ALTER TABLE ONLY public.offices
    ADD CONSTRAINT "fkey _room_type" FOREIGN KEY (type) REFERENCES public.office_types(type_id) NOT VALID;
 C   ALTER TABLE ONLY public.offices DROP CONSTRAINT "fkey _room_type";
       public               postgres    false    227    3303    229                       2606    17206    receipts fkey_appointment    FK CONSTRAINT     �   ALTER TABLE ONLY public.receipts
    ADD CONSTRAINT fkey_appointment FOREIGN KEY (appointment_id) REFERENCES public.appointments(appointment_number);
 C   ALTER TABLE ONLY public.receipts DROP CONSTRAINT fkey_appointment;
       public               postgres    false    219    3295    234                        2606    17211 '   appointment_equipments fkey_appointment    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointment_equipments
    ADD CONSTRAINT fkey_appointment FOREIGN KEY (appointment) REFERENCES public.appointments(appointment_number) ON DELETE CASCADE NOT VALID;
 Q   ALTER TABLE ONLY public.appointment_equipments DROP CONSTRAINT fkey_appointment;
       public               postgres    false    219    3295    217                       2606    17295 &   appointment_diagnosis fkey_appointment    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointment_diagnosis
    ADD CONSTRAINT fkey_appointment FOREIGN KEY (appointment) REFERENCES public.appointments(appointment_number) ON DELETE CASCADE;
 P   ALTER TABLE ONLY public.appointment_diagnosis DROP CONSTRAINT fkey_appointment;
       public               postgres    false    3295    244    219                       2606    17216 &   appointment_services fkey_appointments    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointment_services
    ADD CONSTRAINT fkey_appointments FOREIGN KEY (appointment) REFERENCES public.appointments(appointment_number) ON DELETE CASCADE NOT VALID;
 P   ALTER TABLE ONLY public.appointment_services DROP CONSTRAINT fkey_appointments;
       public               postgres    false    218    219    3295                       2606    17300 $   appointment_diagnosis fkey_diagnosis    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointment_diagnosis
    ADD CONSTRAINT fkey_diagnosis FOREIGN KEY (diagnosis) REFERENCES public.diagnosis(diagnosis_id) ON DELETE CASCADE;
 N   ALTER TABLE ONLY public.appointment_diagnosis DROP CONSTRAINT fkey_diagnosis;
       public               postgres    false    222    244    3297                       2606    17226    office_equipment fkey_equipment    FK CONSTRAINT     �   ALTER TABLE ONLY public.office_equipment
    ADD CONSTRAINT fkey_equipment FOREIGN KEY (equipment) REFERENCES public.equipments(equipment_id) NOT VALID;
 I   ALTER TABLE ONLY public.office_equipment DROP CONSTRAINT fkey_equipment;
       public               postgres    false    225    224    3299                       2606    17231 %   appointment_equipments fkey_equipment    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointment_equipments
    ADD CONSTRAINT fkey_equipment FOREIGN KEY (equipment) REFERENCES public.equipments(equipment_id) NOT VALID;
 O   ALTER TABLE ONLY public.appointment_equipments DROP CONSTRAINT fkey_equipment;
       public               postgres    false    224    3299    217                       2606    17236    appointments fkey_office    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT fkey_office FOREIGN KEY (office) REFERENCES public.offices(office_id) NOT VALID;
 B   ALTER TABLE ONLY public.appointments DROP CONSTRAINT fkey_office;
       public               postgres    false    3305    219    229                       2606    17241    appointments fkey_patient    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT fkey_patient FOREIGN KEY (patient) REFERENCES public.patients(patient_id) NOT VALID;
 C   ALTER TABLE ONLY public.appointments DROP CONSTRAINT fkey_patient;
       public               postgres    false    219    3307    230                       2606    17246    appointments fkey_purpose    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT fkey_purpose FOREIGN KEY (purpose) REFERENCES public.purposes(purpose_id) NOT VALID;
 C   ALTER TABLE ONLY public.appointments DROP CONSTRAINT fkey_purpose;
       public               postgres    false    232    219    3309                       2606    17251 "   appointment_services fkey_services    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointment_services
    ADD CONSTRAINT fkey_services FOREIGN KEY (service) REFERENCES public.services(id) NOT VALID;
 L   ALTER TABLE ONLY public.appointment_services DROP CONSTRAINT fkey_services;
       public               postgres    false    236    218    3313                       2606    17256    receipts fkey_specialist    FK CONSTRAINT     �   ALTER TABLE ONLY public.receipts
    ADD CONSTRAINT fkey_specialist FOREIGN KEY (specialist_id) REFERENCES public.specialists(id);
 B   ALTER TABLE ONLY public.receipts DROP CONSTRAINT fkey_specialist;
       public               postgres    false    234    3315    238                       2606    17261    appointments fkey_specialist    FK CONSTRAINT     �   ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT fkey_specialist FOREIGN KEY (specialist) REFERENCES public.specialists(id) NOT VALID;
 F   ALTER TABLE ONLY public.appointments DROP CONSTRAINT fkey_specialist;
       public               postgres    false    238    219    3315                       2606    17266    specialists fkey_user    FK CONSTRAINT     ~   ALTER TABLE ONLY public.specialists
    ADD CONSTRAINT fkey_user FOREIGN KEY (id) REFERENCES public.users(user_id) NOT VALID;
 ?   ALTER TABLE ONLY public.specialists DROP CONSTRAINT fkey_user;
       public               postgres    false    238    3319    242            	           2606    17271 -   office_equipment office_equipment_office_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.office_equipment
    ADD CONSTRAINT office_equipment_office_fkey FOREIGN KEY (office) REFERENCES public.offices(office_id) ON DELETE CASCADE NOT VALID;
 W   ALTER TABLE ONLY public.office_equipment DROP CONSTRAINT office_equipment_office_fkey;
       public               postgres    false    225    229    3305                       2606    17276    services specialists    FK CONSTRAINT     |   ALTER TABLE ONLY public.services
    ADD CONSTRAINT specialists FOREIGN KEY (specialist) REFERENCES public.specialists(id);
 >   ALTER TABLE ONLY public.services DROP CONSTRAINT specialists;
       public               postgres    false    236    238    3315                       2606    17281    specialists specialization    FK CONSTRAINT     �   ALTER TABLE ONLY public.specialists
    ADD CONSTRAINT specialization FOREIGN KEY (specialization) REFERENCES public.specializations(id) ON DELETE RESTRICT;
 D   ALTER TABLE ONLY public.specialists DROP CONSTRAINT specialization;
       public               postgres    false    240    3317    238            �   q   x���B�R�D�/�#{y%O	���#T"�!��+d��ȖA��RqG���X-K�q�g��0I�K8mI*�)����)���+&'�$�`n���Lo�y�;Կ��      �   c   x���1�(�){0�s���z?]���58k�^F$���1�m%�`%�,ke�ةe�zQ��lqz��4v;���ǚc�����R�{$}�*T      �   h   x�%���0�PL�6��%��v���@6�� w��_���r�{T�Q�?B#�5R60�"B.�c��(�%EI�ּ�� \��b��hr�۸��T���~�g�      �   �	  x��W˒���w�{+UEQ`�� 
�Q�3AD|��������>����D#�̵*3U,nA��4���-�<��_4Mwh�E�(\GDH�Rl_n�z���Z>$�v|���6��]7�U��5��H 2w@��k���҅T�s6�	7ન&)���ypU)j]a�Ys�<܄���z�n��(��a,�l�Y�;�9�Sb��*M/����M��؛<����bo���OT�[���E�|���P��Վ�"�[)G��Iix�nG�؟i(I������j�E�@F�0��j��_�M\�{��}�ɕ9O僾P-Sy��A�s���^_CK���^��2�&N(3��"gpy�`�E[��;K�+Yη:��鴂C�J��h�L�=�׼�q�0��F(Cy��f�?�ժb$������&jvrlf�)+���f/���ݲy�������*����U:�g/���hW�}��l��'T�1*k�z���$��x�/��IȾ�FD�M¦���� ,��1�H�j�D:JR��U��l5���XT�,�a��G�Y�n��~s"&�t�a���OJz�ؑ��H��|���i%�й�I����o���7���ރUv�KQ�P=��.�I�%����qw~��'������F�~dH��Z͏oE��%�3(gn;�}�-���t/��d�.��Cߍ##~��/L��V��a������:��{���V_�K/�`6�'tfi�-��2;с�fY��3(,#<���J8#�O���b^\M*���<�@ND�63��R�Bǳs:�,��*�F��Z�9�e�|pv��寯:7����b��=��N�gV܉~T�с��S���PqE��2*�i�.A׷�;�r�?��GN��%���N�h�VC*��l�屖1�F���s�?x���{���^���0�G�D5`Ve�ξ{YI�?��K���j$���|"�>����%�UA]J;�/K;2� �,�/��Yd%�������������w;_�4R��&
��$o��ԁ�YXL�ʕ���yD��={v���6Oo��^��i���.u(�\�B5w˳LT{�!'+��ʍ����>��i����h��9�����x+l�����h��,�&���`�_VQ�Yia�?wD޼Vs�31��2��T�秹)HPFt�N�z<�ù%Y���=:, 4n��a�Cu�DRdȗ����Y枖(�nJ�0 ��k��߽�y6��=�: 0n��yGg���4��Bϖ=��l'Ŧ�ZcG>ÔZ�[Cb�E��K!��$O7����?��F=;����	����$�����r��c��Yi�7�����u�$z�11��:�b>��H���\��a:�mܵN���`{9`B�ž:�W+~�[f?�wg?�vfU7��ߘ �``�	��6����
Ϲ���u�8�y\�M6�1XE'���V0)��ߣ�F�k�����i�yc��������9Z�Ji�oU ����w�=���o�D��ĝ��mqH#|M}�����A��c�F/���y����^/JyWqt�X��=��4o]�C{$�%�ݘ��Bwm�����[r<b��Ɛ�@��m�~���f��9<�:w�v]P+��7���rk��N@��醜MZ#�¹�`��7\�F�'U�=���,�(�=��f��1��Ā�L�������"[��lY��A�P$T�l�n� �U�(�������]S�Q��!�@������G�[(���1e��̤=�:8��N�r�pu��>�tvL=�b�[n���(df�"՘g�礪��6�޹!�,�?t�	�tDG�j�>M�I���7�@y˓��؛���Jx�'�{�4��w|�q�|����q�"�&y�+=�6��ٯ0���㽖xչ?}G�=t�0\���N��;��)� ��;Oo;��s5�Q��[��o�}����ntA|C���wt�y�UV�iݧ�;��"oY��Hs��<�d.+�b9�:�aС1���|S��oΏ���Q�fej��-�T�fa<�Γyq�.s���!��1�ls�QQ�����zt;������2ի�F�y</�S 9�EĬ����:�ҵ��C�,��w��<���3����ߏ���+��E�O��\Ȅ�U����P6׳uq
Ӥv]�hh��N�A��\
{�U����O�jT{��j�n>����î6��/�?�!�#�9S)�b��(�/#���v�����B��V�;�v�����uy�w�Q��6S��3a�E߲����Q�7ת��)�N,M�ٿ"&� �5ɢ�ν31t�͙�i�S��i���-�$�]��K!Mݝ)X���G.d-�kR\�y^�{�w�<�D��C��+仢��
Ӥ�n�����}�H�m�}-�\˽p���j Qs�����;�v���� �I�      �   �  x��V�nG=w�-�8�cbX����1$�ŉ�ȉ2`##�e4#R�B��ի��� �D`�����W���V˸~�G~���M�.w�������Ux��ʏ�'���{7߲�ً���V+�z�	B�n�K��p���/#9�����׌n�'�����:mB�ܦ8�g�_0.`���T��ȍ�PF�!�3	�J7��p6R���_����Ł(��ζ�^��NP�/`5�d�8��Z�KU��v�n��.��� P������t�t�jJ.�(犸��e{f/�$hL����a�f/k�N,�Nn��71V>������xZ,R7��>r3�0���VX�!΄~ס����sA���YSgX���"���!�j�Ʊ��do�2b%7�[��9��o�c'�2,(%�ˢ����%f� F�8C�u(A��SX�T��a�H�[f��}Rx��4������@�@|�u}b�e>l�:(;�n��e���Kp��@u�3��7���4eF�j)�����6�U%��}D�<�
�O�ꡮI匢D$Z��y!��i�ѧ4$S�t��	����дS/Ej�~!¾
�Z��*���5{H"�)������hb��N��PKu6�r�$�ՖJR���*V6iD5�%�t��yBd���^s:�1�fI|�~y��ܖnM2���DB铂���Z��(Ku$�8�N�^��Y�ě=�t��LŚ��:^(x��"�:8D0HM�k��mU8�ؕ��&]s�"��kOz�(nm�7A>5I��.k�i��7@�͑��j�2Q�*?����c��w�TZq}ŉ~a���H����K�5.u/1;�w�P�����Z9L���F&\�.��tZ��i�X�8.y���:\��[�&:�ifv��K�|�feĂ�|�y�0\�m���� pT�GL� �ɣ�sc�� �t�e*�W��E�k^�cQ��< BzfW���W�O�g4�	�i���𽢸wY0Ȝz˙�8�l�
��qZ��] ہH�rM�>Ḱ������޳M�����#\;��2�}S�O=��!�	`�hy{/��J���90�8�'�<^8���7.�c�����p?&0ۨ�9�Γ6X�r�f]��ͷL��i��9���9�����Z�@ҩWSE���k��9�YQ����-��W�+:���p��P�����'͗��a�Z�/f���      �   ]  x��Ykn�F�-���`����0k;I��-�]����E��8Ql˹��F���z��h�0֑fȏ�9�,�8��8��8��H�����}�oiqk�s?r��-���8�5V��V�"����O3�ً�Ȋ%KL�{��f�CEMV�(�����e	x�U[H:�>Z��
 ��Ե}K|��_�H�,�P`�$�El!��3�q�Ȣm
��dl(��W~��G|`=�&��ƿd�U+_\;��ߴ�v�oY&Myſ�_���|'�`��b�t5_��Yl�ȊO�Z�+���'�p��ϭp���
�6:�\�|U�E��ڑ%���]��{�B@ږ���<Ǜ[Q�񜪂 !���L<��3�5>9-'C� ���nV�º�t/ϣ�ݵc����t(߾�0O��g��΢}��#rhG̼<�G|���r<H�5mS��V�&����\{A�ȪhXQ,���p�T `��bͰ�Yd8 >���Z׾�Ŀx���*<��(:�8]P�����޸,��R��_jr�&>sX�ጴM%>�u��<)M*V��E"�<���G��tuR�t�OfW�N�(-���5���CRq;P~�ʔ�"ؔ%*rCJso�B��Ή�ȇJ_�ǞUՃף��l�f��~�� Ɂ�W9ב=	P�Pq��+&c<��/f��{N8�╌L!�X���߰�*rb;�Z�g[[?�3�"E��2o��K����*Yp���(�X�*pTƶ��e�W1*[��1/#���@��3W�q�i��E-!F��t�#j�6�ƨ֗���s�H��
��U����%Zt�ױ�TڂyB���k�҇�.�"+J��"�O��N!y�����w���M#�΅gNkH�atw2�P^�������מv��#o��-�O���'6��o>�Nɀ�u9R�����WFt��%�_��]���%�ǫ�%g"
Kߠ��`�.Of�T�G�P�A}�G��}����/�j@��kYU��q�0͈8�h �6�(xdŊ?�0cA��G���V2��#��+�B�o���C���K5*!|N��}�6Z)�褐j!�x5����"1������VT4��cH}����?25s���LT7A��<C�(�]$St�_hR�'mߐJ��`��d����C���]��|"2�1SC�x�ᅌfV)���&WH�ǀ���ޢ���/���9����H05O��x[5�?U/�*�S.��5��:Y���(�����l'/��ˌ�ڕ��@Y��W	���L��;�+:�����ߜV�)9�/yգ|q���i���8`�6֖.!���wt����� ���ۃ!Ez�y6�h�zmX>Cj6RC�8{�	�\o�a���p��R1���cz�����uS�k��\��N�l���U8�<�Q	�q�vP���Nn���ٔ����:�|Nlǽ�j�W�?��e.���{��#�[���mL�\&N
�Yw@7Lɬ�
h�0�1����
�hĪՋ��Y|�C�55�vj��8��[oY�ߐ�Ix��a��vP-���s85��� GNϭ�V�$)㌦����vd�����e�:�*�G<M��S�t�\۶��?��      �      x������ � �      �      x������ � �      �      x������ � �      �   �   x�5���0��0E$�N�K���� }��L�ǊꬨAVv�ǧv���b\��QȎ�;&���ҕ�	�	uٲ_5��T;�1Y���&�n��_����J�s�~��v�7#�r7��\E�"����
�@
�@
� 
��	��	��	�����e(      �   |  x��WYr�@�F���*�8�]r������L*I�r$#$��s��~#��\v>(����Mo�䷉�27�Ij�p]��<�RH"w�K)���g�6�TVf�Fue�������5os����Rn&�l��1l�/�[R�$
*琹«
Φ��w��8*�B%�m	=�$��aO~��6*�*q=C��6��B��oA)Z�Ȓ��|[B6;���/&c\A�	�ڃ��f�����9(��T�5����\ (�Bq6�+�،�b�k%���mD:q���� I�q@�ӭ$�7��Z����8Е��Go�X1�1��'�(����+�#�Ę�'�sݱW�k�oWMzzc�m��@����t��K���05�"
�:|������ٲC�Kx����V�o�PRj&����ix�>J�Ǿ�"�%�A�׸\j��Aٔ�,F|62�w���b�wA��@�Y_W���o��NX#�J���e�-��A�,���ub&/�w�>��{i���=��.�T�ja5��;X��aPm�/�Tŵ2�e豒��O:$@�^5�4F�O%�^;�����9ժ������و�֘�t~'�I'�p�����!XM9T��O�w��]ev���#�M��7��f
ȗ���"~F�]����������0/_�Ӯ��O�3KS=+5R�͗Ƌ2�`H�����$�&w�zTZ ��uM���<f�A�י&(v��<�OF��xRn�с&C���>ym�'Bs�Ǫ��c�ؙ�����J�Ȋ5t&6���07�Frl�2��Ҁ謓v5Q��qn�C,,�����d��9��`Μ.m͖ӽY>��;q���B{_��?�7�n)vP~�� �w�)V      �   t   x�λ@��9qܯ�_�o�-8<V���Θ*����K�ݱu�G��q��O�6dr2і2�6e­d�m��ۖɷ#s�]��)�#��>����㩤��J����#�|" �      �      x�U�ɮ��Ҧ7V^�?W�}7��=)��	žŞ#�5�0���]�
e�װ����}�&k!�̵�����ߞ�PÝ˰-�,��̓o$.���<�śsm�8����*�^hg�3\�� ryq�1�Ems'���R{!-M��iw����	��Z'����UZ�L�B���� �{���<���a��T0쁿+(����#(�$u�(�$���[w#����-�Jb��#����W��we�H7�z02�9�^A9��@+�=둊X�?�ك ��~xB���=G(��o�V�Djg�)�Ǽ���[i�3G���4�2��8��@P6�@n����S�>��B�yv[��sE�53`�f�ƸR�J���=��Q���ԏ=Ύ��r_��Q�TH�L�Y�Ӝ�GiF�g��YfK�RbH���rF�Ҩ�@�`@)���^����0wq�!3e|�������I���3}E���x˯���n��D�h!��������G��A�t�ۦ��v| ���B�1(���+ْ�3�aq>��I
kOI�_(VR��kcק�كi������Q��f�諔�x��?�ۼ��PV�?�,R��q=d�L�ԏ��J3��T8|>
[����ϛ18�����u�bv��]���7��+ˏ����	���!D�2ҷ�������Z���\��*�U�R1\��ñ��D
đ��CX�H>]&�����?SQ$��c?s�w�a�n�6r�>��&�F�ʅ�����;�t���a�˜�0���8'�0O��:��	���G�X��u��YJ�9/�3�z�S��^�}?"����ݘ�&Nq��1�5��)�J�Z�a|���}82����һ�t��8n�)v	A�ހ�bT	<��3Zu��
�2����`��CxMܥi���[�Г�ា�H���G� �)F�K�Y����Y��� ��8��6Mb�yq�g�.�j���J�W��#��)�a���+�i�_	.�g��]{G�h΂�#��� �H�ϻ�EkX�����/T��g�dl �N���,�I����Q�C�@���p�E�n�|:�������b������Y�u���4'�+�S��e�u�yu�e���w���F�̓a�Թ W�MiQ�ʵ�� �K��qF&d��]��m�JjF ���
�"}�}���>�唋#�`o���}Ϗ�L]�����K.mE��R��ӿ�譔ɷ^
;1��m��F��u`�j�:=��h��� �K}��j����C$Ff7�^� u��9�\Ƈ�7�w�ѪN�|�sýG�E�d��+%�P�Qp�:�����X߻t���N���,�A��ֱ��R:w�>m�=z��d�v0A�ǚ7��a��/(�)�ϭ�����]�3of���1������5�g��g�b~��:ԟ��܆מ��b�O|;Ԥ*'m�B*9W,?��������h}��`w�g�_m�gj/m�� �? �g-��n���oA�jY��H����9T���64s����OĬ��3XW��)�?{�G�����/�&&1B`/{��{��Sr��.��`�f$� K��)'�(�����y{��fdo+�����"����g�۰���U�.��ɛ���$�G�I��.��� 5����t�xJ��1��j�P�S��z{���R����;侷	��+.��Jyk���t ّ���ؘ�;{��d&ή����2�V���:�P����<��y�g*/� ��������[��
K�\���m���$��HH�Y
D��C�ڃrq�����A��3����B��4XV��.�͕مv"}P��t���aVL�a.ȷ�OY�0��U:x�)��<9��\S������@2�3�0i��� �	p�O[B�w0a��V�8�k�M���xB���JK܈N�еrDTۺ�g����V�UwE��F����
\���I�(�j�r�,lP�����z��u��#�ҩg9��+~(i>vA1�f��W���ګ�/�����u�w��)Q^7���
+`�Js`����j�ڎ�xÌ��=�̪'�>�P�TH�u�`�`}�L�=��
m��ي��P��
y�g����Ǖp�@�V%a�u�~\���{���q���1ϊ�}����`��џ�GpJ�b�;g�*��g�(w.�_>�0�(�s�M)0�Y��f���\r�E��ߺ�h��=�ս�K�T}2��Z}��`�)��̞e�s
�Kt*�!P����eL6��aA��A]��ߢ��e�9z�D-ȏ
X��N)��d�L��^���M�PL���٧,1pz�?�EF��:�E�e4�{���rhD0��-H��֤�<_�W��bsl�}nx� �c��"�FinW���P��}�eL�XC�^��O��$���]8OG|���+��Q &�_��?竱ͅ�C�C��y-2�iz�k~�äm����p1k�V�pzw�\U���T~�g�͛��iP�:�r����a��$�}��qh)��:^Y���UͿR��T����������v�ue�wo�!�W��/s�g�d��jؽŢs��A�k}�v��ڼO�߽�q�V}�d�����/�� z7�s�@�d!�A� �k�n��1�b�b7����W�N">��>�V�ukva�qr��WD�/>�3�Q'���}���h5;�;�=�N�A��|52�"=�8̩��6�_	�/��C�3Mŏh���?o�K��ό=���;��-�X��^;O��z:�b��%m��� ������J/�S..��\��f�_y�v�X��$�2�@��e�{$�L�G����r
��l`�|(�f�K�_-�m*2�ȳ�C���Q�z�H��i�c�s����&׭��%�>{��I�3��M{�sۊF�Z�<j�.N04�ʔ�O5$J!/���8)��O��2q�]�_��q+�����X�8f/�0��j��� ����Fՙ�5�p�<,�ݿ����[��L�;ѷ��R�Q��<"��"?��C������
a 5Ο���Wz	�[��e����o����(2����zu�+1L{8�R��Ь�� ���:��-�U�֒`�ז`4bⴹ1�C��P����e+��ѤVՍC�&!v�g�.���鋲XaԺ�D�ϗ�.�h:('\9�糴��V|���N.�<�\��������m���I^�[?��#�xd6�s	�2�P�[�X�L�d��2�<�
��>-�U�?����f���KLY���&V�T!�Y3�7�g/��Zs��i�Q��lX,���H��$�:�|a������4#��]#X���:G6���y���#<`@����%Ƶ�dAv�� �$ߗ}���Q{�r���B�8Wh�E�ֿ� �d��c�{���)7��$&*h���ݚ^,Q�([$G�zಊk_�1*ye牅~3���`����] �% ��ػ_��j	�=�0�|҂�!�fJ������Q�Ӎ$���t�^�1�{\�mT�n�������T�e��D$����,��N+����S`�9��ۯ�R�I�~' �X�_�؇'��_��h_"��uu>��(��(�Y�!�EJ�<w�5���,��'M?.3@�
ZB<Eԡ23S
�F�햄��Z�8
.��ڠ��`)/V��O!|~,�Cb�E	���^��{��ő� �[��<���W�#+�Ɂ�p$�y�P3�?n$<��1�_���bs&)<'K}�)��;��Y���=��^��^���P�����T��<�P>{�-B �Z|]�0&��^���s�T�5���?~���}����зzG�������(�C ���%ψ����w�^\lN�o�����N���K؋	�H�h�_m�!�է����(�N�����^�SJ�'�}�"V
]q�S|� W�=fY�K\��������/��_��?�������������o�-������Z�Z��_��������������������_������~�����������������������������     ���?��/��o؏K=��Z��2$dف�OՒ�Ѻ)EW%�t�S��a��4<�$;�sN�x���(vӿ˭�D��L^Z������#�dH���[*��Ps��h�	(�C��m1� T'͸¥�׬5��ˑ�k�<��<B��8'���ܧ����O�� 
���sp�;��*�h>Qfz������XA�dA�2����6��X�j� 	��*]o��#����>j,����sK֫d����5CU�o�&�Vק��i�Y^��)߈h��,�6��*� �'� �e�o�;î���g��kp}�{�/ORR��_u�����ۈ�t� �"ڈG��z���;ݼj�K�7�<8r���}Pg���H]V��|�5_��	���ɜ���g�qu ��\�#����~>���(��F-�p����t���j�gC����ɐ@6B��ঃ���z,��vy�X�D���[���w\���|i�9��p/���w���U��lܔ:0�~��B�ۓ�{o����܂�ι��W��N�I�HǗ�!���=:l�t�+%��`w�'�녑�A��4����6[T?]]�Y��;�c��N_�8�����7N��v���U�G+��U����Ҧ��/�U���c���Ƽ�.fJ��)a��E��W썛CĨ-O4�/K�d���A �5�_�����q���w�˜Z閁�S������h;��>A�v"ҩ�sSVT����hul����ty���)r�Z���)�x9l3ɾC�|G�@:�j�X9
`4�)r�������؁s �;q�R����8�N��fo9F���5Hq*�y�2�W��$��`�ϵ��
��=���:��=�*�2�=J�Ȟd!���a�?o��vo_<�h�4�~��Y�� ��H=«F*<{��LS2n���f5ZEy��I�Ρ`�[���_^wO���<=��c�q.a�W���"�)�amC��q�Q�%��  $�Ö��+i �:�XrE��jE8?rT�n: ��>gmc�b����%�]eWY���h!�����jQ&�3\�T�x��kڬ�Z��i{
;M��\�����6����J}�6���6����6?>�A��!o�~Uc�N� �RP���:�����l	���%�|��	�/}jd��e��c���^�;�@t�l�h�T|��=/���Q���ԷzG�m���콖����o5�J��ړ��[\[�	&�eAN��g��%"����C=�%�٢���@1n�$���QR���s��':��6dyx�=�^����F;sfً����~U3|��9���-�X���fd�1y�Ɵ�H��$��T���ڡ��-y��jA[m.�Iz�ڧ} �\�O��d�������6�·�Ϗ)Sc$�#���>U=.{U��}r޻-T2��@p����Ҥ~�y� ��$�����\@�5���o���/��?'ڿ�_+��io84�C���]߈�T*oH�����9+P�;_c�9��t��f�>Kڏ�wyy�]=�R^��l���իV������󁩤��N�9lM/l����X��
�T�ꭞ�8B}~��X�햰C���������[lȪ<h���	��:���I\N$��u�Nϸ����X&ՄqA[�Ŀ]���K�>CT��<y�aE��9�pPC�QP�����"�X���n*���3^�.���HI"ߵ�Ӡ��~'P��Jyb� ��Kʁ�?�S�O��P��[�qg��7�����R���⽱��!�s��]G��W�`{V�|������.�ӸQ
��XN���w�p�gҳ��-��&L�Xiwq�d�#IM��k���]�.�l���a��`�B��ZX�o�>.X&�<�v�O������z&u�O$�����ҊL���´�^6�z,�G�T]���/`�!E��d7�\��m�w�Z�y���H����UW���~'L���jή!�����g�s����8����4����o���骯V?�;&͠*5��B�!(��^�����CuO-~S�ԭ��a@�"I�� ^����#��Q�������~���X`�Ro�arPTc��S���O Y�9�	�Qj�����v�B���e�}I��O��xQ�;Mi���?��,���TjL�M&F�$M�af��]�6�̦��"��o;9ߴgCT�Ɠ�= ����a��.��$�Z*RD��������.A~G�S&>�|�y8�@�P8�������?j���CD��%�\ �r��[|�ջ"¥���E��N2IωA�&�S3d�} �̲��&�\�J�/Ar^������K���Lp���G&�{������Km#C��Ly�8��˶�f�!d����+M�$�x���V�4u�.��&�5�ʤ©!]riɾ��##�4f�6X{w��"����#6��]��Є�����=ڰ�����$��H
�y{����w�z�a��p9�I����4��)�J�0�Z��}�ge2�\�X<�mTUG��˺Qr�=�[���5�*�F+�G�5]/S���Z�=c��S-���w	���W��������_��}N�������Zz�j�����EY�OW>e�*�Κ�쉽���=�;�e�����.W$�T[A�Tܻ��o/\ޟ �r!��2��ӓ����$��T�<O�[��1t�^F3S��F�6kS�>���h�����X����_�����Zy��~��:��x��Q�8�}$��&��^��ˆ�W�ʃE'5<i�/|�{�U�)���+!T9��6����H>�t�W.7݌-m!�n���gz��� f�s��0yS��E���A���A�ݽ�ϝQ��pŨ��>���J���|�'��
v�i ~=���z�kvR�k߳�A[��é��ծ8������#������8De�@U4�J���7?����;� lXgmB�޵���1yvL�}��Kf�����������/�ms¶i4�����K�-y�O<�-���rǞ=j68��&��/��@}4�:��!Y�������=���V ^M�U;<="�k�a�x�G��Ȏ/�S)�l#5�{D��0u�H�-5�����l �����0��&���{���Cϯ�^P�t�<]:No�g��N���'��v��:���fY����.Oð�5W�m�����z��=L$.I��c�'��a� bɾ��c���7�,.�f�L�>�Y��9�=G_R�Ep?/qA?����_����8��2�� �-s���,s�.�ѓ��@Z���_ޑ�|<h���iU5�j[�cն3?��4� ȝl������q��"�Jդ8��V�D��Y���@?�f�ey����㇔��q}�8��e2ɤS���p��Q��jw@F�ܾ��^Oi��g���f+웚c[�$��Ԃ����ut{�s]���[v�9#3n��W�����a�خx�A��.�&����ʋ��՗�b+eQ�E�+��~���r���N��_�/�.6�q5�<#
Uq�Gc���K:4�&3�-�~o�̹J����r@yL��o��ޝ��Y!D]z���l�/odu�kY�Q�Ok`#���2�,�-d� �Ӓ�wV�6,�kA8���@���ǒ�#��Դi\jܳ*�b��6�c6�J��^j�#��iq�US��4��e���߈D�ٻ/�e�_����n�<�Q�:��{ށ]g_Z|��i����[��`(��Kv�{)��b�$�jJ��C�M	��׻�)�~�wՍ��7��~�N}����
�@�#��0�:����������N4K��-xXʇ �S�30o]���2M���[(���&�C�wJ	D�d���3I������
�eՑS��@�yul���������\�D���#���<|����1~����$��m��͙O���l�c�ɻAd��ƸY����e��c�HV�]d�6{xCy�@����Q�d���$U�	�^r7��p8m����~�~��Io��m�?�$��~�-#�ԩ��K� $6��q�����	    6P��<ZI��U���{���֭��w����Ď����6,�Ғ;�z���bM8%���'�.5�S��8��&Sv>޽�?.-�dbG}� 0�Yg/�5Ǥ i�Q�J�x �V�WQ���OZR�[?l9�~ܞ}�A\�\�~Kj��q�W�g�ѭC��I����(*�*ⴾ�{ʯ���|��)�E��� �a�3�$�2�>^�.V�*�^��g��3��/�C�C��,�w�Kpȧ�������JfUO�y���8�2JB�-��j��%H�H1FX��o
��O����a˻�Z
�T]�*�IS��A=�B�c�S̲���ާ��q9��P(Q�Qu�F#��xB���u%g]{th_��n��0��3\�v�q,���k��ֵ�O��v[H�^�a�o�K���H_��y<�?o��ɍ�=m[����sXE��P��Ԅ+���@C�c+ڄ)�_�0��?�*_j��R���G�4W��v�a`6��b���I�lT�f�e��<zeQc/5H���DU�V��
>"��mq�<z8^�3����n�[�����Ҙ�`�=��"�q���W�
&���>I�x�5��X�J:��;�^7}X�w��؈��N}\�W�o�@?�o�|���Ǫ��� �c8]G���x�O��p�y��(oΛC��ywH�.L���v�x��C\�#�yS/�0����x������-�����������7���=��f���p~4�e�`�5��X��-�B�ħH)�����/�F�`��xO��/�N���P3XE(����ra��zx����fF���M���	%m5f/�F��O���)6sfj�#�Hzo�VY'��. ���K7�|��QA.�A>���y���&�닦!�8���Z9�<�.C>�1�}�^[�$k�*aA2��C���y���n��dr�Mr}�Ή��C~W�ǽ��9A\T6|&�o�{C.��q^q��z�3��O��uQ����b\X%+mi	�eI�����UJ���%Bsv��!y����q�	�I�|�>����.l�؛}�@�iуiB����3�9�E��� �a��D��3?̀�?�v��4�p��L,C��0u�C.�,sǞ�Ϊ�+�O�]}�"�J�^8���c�$����&U�pǕ���wϦ�B���H&r�!��)_�W��
�y����D�t1������s���
����+`@Z�Y߉�K��x!=��:~'�מ�����/�9�ǟ����{my�6x�~�Q܊i����cW�jPᕵ/��/LbԜ�������&��}I���� �����O�^D$�E/.r��s��T'wY��O�𮄓�ٹ����r��,}O�������s���c5G�w�x�pW��?o�Kf�3.�O�p��Rي�ﯔ��t]ߺȇ� �f�姄r�x�	�S���֋�tX����ǔ���^1�"P�ϒL��9��6�SU���m�N����럿�?rф"�oQyٵ���lm�x�Xj��1p�*,�Ƚ�CZ@_��
V�d�cQ����
�>R��#?���7��#��P1E<�~a�r�AsQ&���@�?�6�8�$9�)'id�.1����6���������!ϠѿM��:�JJ�0VD���꩑S��V>�uG.���Bf	���eΈg�����9��p�$x�y��p���=��lb��%^�ˍ��˱WG%`(��t�L���_w�~G���9�A>��?��WaΈ�{-Eg�Xmm!�Df���<�8�������~pc`c<��]1�^�Vb,>ڠ`V2w������i���13u�'�%K�����P(Z�2�cX�Ƃj�05�ܓ�m�pC�Uރw�צ�L��ک�y`�F�tQ�j|Xč�lk$3�&vB��+��
�7��w�z�NN�S~�w	����`�O�)/VW�����0�da �;��vb���L/^ySj6��l����/π~w �e6�}�*;�y3
z��b��wt�5ڲ��opqW5��>q�2 �j�ò^��-]�,�Hr]Rr�t����O?ٗ���͋H�)����c���B+r�����U�A���\Q�@��/g0�˸?�y��5�8ȣ1WM)C=���R�Q�(�jv���t��H�7�h��"��2�H宓�����r�Ԙ�KJ���.�ጸ���9��ADWr�h5�
.[ڳ�M�
S�|ď���$%XY��r!��3��D��5 ��t�*~E������*�ε�&f��
c��|%�(B�t��`�2E�>l��{� c�B�A{p���XQ�zBj�8l��"%��l���L��4{AtJ�'�Hq)���'�i�����}ܹgݥU/�-f���;Y�lrq%ݍ��N�D^mw����043V�C���!�߳��K��!�S�s��W����kp٫�;�=��۹^�� ��槲@v6�(�N�����=���S���� �+��п�2�o�ć�u���_W�G�|�����@)��#^�|}"���lm�} YN�C����WTD����Oܟ9([�����=��aGQ��{I/5���_�}���8^��Z��U�1�e�4�*+�KC�A������M��vϽ��Y�w������RDD~���c�<���Ɨ��<�����Y�؜Na��(4��p�`�����fz��@�Ƈ���du!��	o]��8 ��	��`�ޅ�l
L�q
����1�Z+�F�����K�%�N2�%~N~G��6��l�x�
~��O�Ȭ�"|����Tb�m��+��d=N8f�2s$MQ頕��%�B�s�1�l/I����;I|!���5/I8x����cd�4r��%�< �B
��A���Ӹ��!�Cx֑���Բ�BQ��<�*{�������-�<AT�����τ� 9�w��Mu�	�fT`��z�陡��+���Hu�t�u��l{�j<T�$�qt�i۫�}�N:�qc�#���j��o_�ۃ����kǿ�_N�1���Z���/W֑�`�G�@�$�yA�>��
c�^Vd)nC���l�^��D2]r�e�r������؉w8Q���Ę,�d�1 �#�p�c��g0,�h7]��)����9I�]���ޣW��%�X-�-�T!����g�bo[7D�U�a:��
��,��XV[�f?��bN�����tr5vT���(!ȅ��*�;Uy ���,CA���@{�3b��g9ߟ�5D�Ae��u�3����%G�t���F��<��3 ?q��j���?gsu,T��M�U@�g�1��9�O�3���ӷp\���W�MbA�V�Oߡ��j��ѺC���颚�3@p��φܧR�{T'c!�'��i��<�qh��ⷉ��Y���E]�a�݋�}�w%;E��ėn~XY�ص$�H�h��J3D�zͤF�|�6۝O�P�����5�I'��ۢ04�޾=�fe���xs]�>�<���̣ҙ�� "�m�;�TO��U �|�Ǹ[ᙌ5����OP���=慾[�_���Z�F>���(�BS7<G�]�_����OT}�\apl��}�����̉P|�A�/�gl��O������3u���C�d�D�9����^G̷qĥ�Z��k~?�~����*��wW}����m/%�W3���ERfv�6V�o��u(|h���t�>�:^=�I\�TqN_�kϑ������R}�2l*�b��`�����Abҭ�BS�
�:'��k�1��#�'e�L������gw\����\y;Oo�dÿ�~�y�ߓ�����w���f� j���hM�Ŏ� ǈb6�y>�-���,j\�NB���`fh�*%� u�R(=�j�N��q���{I:v���-A�;M ӗ9i���6��\����+A	����������O�z_�*�#.��=d�vf�$>5O��﷐��-h^bFZ�_t}nb��3�����*����q��=������$X\�<��i1U���q�tP{�6�hL��2���q��4[C�[#ȯ��w����~h� 8  ���q�H�G���}���(\YB���#�|�����H��m��*UW��b��p�(�O5�f�ڀ��$��.d�d�ـB�	��,�1�/ف�J�r�?:��0o�)���Vc��!�\�P\@sحx�w����|�[<u����7ƥn=_�����yp�#a:��R���i��j�2=^�
��y�y��n��eҦ�����d v���r4o'!gI�p��x}�l�6��@��O����kt���K;�g�-'E �S �g	��:����<W[}�=���e1�GsP�ըS�#r�gj������͒�x�4u�Y�Y���Lq�*��~fٸ6%W����ygDG�U�G��8������4�IE`�1ē�D'z���ϛ$���U 
��'a���MCm�>}<u�)� �W7!0i��M�۠����1�4��髁���;z�~ڵ�s�2�K�,6�m��U���p,#C'p�9�����W>��.����k�s���ԛ���pL�%y9�j%vՠ�h$�S�,�>�5����~X�7=�4iI#
xq��݌�ٓ����p�X�p�����=5���<v��\����fW����K8L��.����J�NO��k��8���h6PmgCZ�*��������/ ���}�K��Q���"[�f�5M����%|���Ѿ����2	�^��]��^�Y�TI�j
�ɹ\8����0�V{�7�X��o0�sB��2�8����.rd�b��wT0��<k`5i���w�IN�����3��3���j����?~��� _"�X      �   A  x���]r�@ǟ�S� m�ޥ�!Ii�I��t��>tzLp�0W�ި��6^�٪0x�+iW����?�P@�A���5>�!�`���P��0��;(����'�����|?�_hʃ=������~`���mh������7>�0>�g��׳�b9�OX�`�`�oq�<=C7%G���f�O�n:������y(������AS�vTl�)~�;����|f���_=vK'>?��Jm�������h[��S���з�ƃ9����-���m?�o�������.��I�M?PD�r����J
���{*J��VY�O�U��f����h���L`3cXI�ҞW�]M��f��;�|CYxU!S~��[�� ���退o-gc�W\n9����@�Vr|�*�*�J���m8����^bV���A��R�����-;�A�R�\?�;���B�m�U��\�sB�*o#ǹ�R�v�֏���{{�S���]�8],(�y[���YM��>��V$Ij7Ӣe�E� ��1+�$k�<3g���;�������Pnsx���O=�%�*�S�S��A���������;���j�L-�Z̘5���EcE,��3*ʹ���N'�ko��'Vpzz]+��u���ʔqG��']��h��.�"0q�L,�JyS��txq�����-���e,��H׷���V�''�ע%�͓o>�!l�������r���{���56��v���S�"�m��;��j�h�,mw�ߋ{��t�@l �fb1ULl &-bŔ���PL3z���a���@\@l(&ubC)0�)���PL��D��PL	��ӌ^b#)aFl$&��D��HJ�Fb�`&6S���F��϶HL	\���4��X)aFl,&��D��XJ��b�`&6S����b"�$6SWbc1��%6���Ċ	��&b2Q'6�����DL��&b�0��DL$��&bJ�Jl"���&R�0��DL .!VL&�Ħb4t��)���TL���D��TL	\�M�4���TJ���	�Ħb2� V쏆nb��b�`F����_E��T      �   �   x�u�;��0и9Ǆ� ����k�"�Q��A 6!�ȑ�_f(��,�(�/���	(>z���\��y���f�%�W.^�0[��i�T�핻L�}�H^[T�#j��,|̡��L��Xo2�h:.t��A�[��&�}��Br�����җ�P�ڗ5RE�-���D��]�BG��͝�)�4Q���T!-���`T�Z?%h*[�I_"�f��      �   C  x��\ێ��}��b�Qk̅�˿�c�f%��(�D�[Q���h������B���x���f��ó�kWWWW7�g�W��{�����m����x���m��nG��Я�{�6���u1�|7��*2�o�w���x嶌=���3~�?�I�l܃�;������gnw�"<K����L2�m&��דR�"#�nJ�������,�'�9����^D��"�g����xC�Я���+�C�
}>��Z�2�Y�"B��g���{U�^wx��ݰ��HrB�}:��?�G��^�^��b_�h�4�L�3lC?i��/!.�זޒ@S2�YL��!� :ޖCa2?/u���R|�*>�?��7�[���1ڄ�Ԋ�gbw'�^GC����3%_�L&��۷��ϊ���ӆ4����B���°�p��?��x��k�\�m�(��m^˘�={��,���R��3���a	����g�"�t[��D��ㅌ�f�(7B�j�y�~s�l���6�pv�r.���^� E���@��0~̦�m�z,�����L�(g�Ϣ*��le�GЄ�+��#˕LR��U�1��)�"Sf���eV4h,�N�d�G����X.�$P����b=6x>CͬA�0 	��A�ieM^b���gN9H��Ջ�)�p|Kk�;a8�Q[���XIC�B�����m��#'0Y!�á����_dY���L��'�M�m�D�"#�S�ܪ8l�F��}y0�6���	ג�@�,� d\��N��n:�??�Jk9��hb�^/A/%�r0Q�.h���{ū���g��V�'���<�r��i2��:���R�^�� ��D����&��z&�#?<^��ӹ��adX�H^�aĆ�%<�����a���/-�2��$�7�$�)��Su1��Tx�2B?�Ѽ�3��B��Wn�Dq-ࠑ.�Rg��4��*����t�*uW�N��G�6�H,e$��Ya��1���F]W��Nd����i�~���.c_�&b��}��k?K
+UX�&��a��+�{h�z�}Y7�S�`�I���~�Y�O�Z_X�lk8��Ճ+� ���^�B�[w������N�뀅UD��%��uULs�Ijn���A�[�p�1
����6��HI"�>�U�v� u��B츿��GW�!6}�����47:^p���p|'*��R���Ȥw�X�vSfmCʒz#]�;�JA�������bʰ57M^ٗ��t[q�7aaa���J���j�,�m\�f�!�o�犆�^�E@*x�ʽqk�޲�οWU������#����&(O� k9Axu��cim�uL$}�ܵ��\�42Kňsa��#���F�ŝt�� *lE?V����(B��/��>n��:u����)eʃ�f�%��q/�(�;�K�eYА��zA(S����ѳ*������F�I<��렵^��I�Y�,�a>��e5隄|�r-�������0�뿲��oh�azEl:7�R��rVS�>n�v���8:�<��<���Z��Z��*	�#{eB$X{*���UY���� ^��-U�K1i�=;��3'���2�0_��i?��*�S�|� ^�X��?��]V�!t����6v3Q����k}�p[��B78��� c&W��e�~6���<�ֱA�f�AK�}��-�B��u�j�>��r��G�\4��(�[�M����<�e ��uyx�b'y�i�KWͩ�g��r1EͮM�;�����n�2�z�J���Ѽ��#gW!���m��6B��U!�Dp�fb�>ŭJ"@û4[i}՝�yx�� 3R��.�;�L$�������Y3�������%�Q� CJ��fԦ�+��o�e/�EM�*&�q�����#&x��@��O�{�q���;��t�ފ��aZG䝖&��zу{����.���(`�^"�sr�M
�3�֡f��#��t���C6�剭��E�.0
�f�^
_u�t���77��)U�ΣQUO��N��������u��+�A�=��@�7Q�Z=na��*��B�Muk��p���є(�,�[t�v��-���Zb�\�l<�ګ+8��Hϫ�r�W��Ȝ��?���l��Fb���k���Dhr����.�X`�w)����|4)f+O�Y<��������ȎÐ�t�[�o�Vs��me"	Z`�]�;�$@��!�:�R]}P���lA�*����317q��h��z��E�"��dXf�@��{�2����V��$�V�
/V��M�|��� 2��Y�@�\f��L"@�c4וPH������Q�.���Nqk�m�.�������i6�&��9����P�(=��Ms��9:���$0��`�nZ��JN���੊�@'X*��F�Ad2��EͦU^G.�|3F0ޅ>�`c�Z�> ����d�v����0쁠��C���	p�<�Xr#}`��iA��i���/��XC��g�1��h�Z`�c	ft�9�lY��(�f�/��w� ����0�A���f{���;O������KR2፫�����Lǝ�F1~Oѵ@y/�ja�zmG��� �u?�脧�������
��t�u	)�Y�ԟ�6h�
(���|��t;��HBl�,���23�M.l�·�L7��.�+��G9�(oy�� lN��q�@�Nb���5�nH��z�ʤ���"���q��̝8tS]g-�-G�Kk�a�%�}�s`�%(�XTIYO�. 2_ľ��cq#Ia�^Z�jY�L�V�C�}i��ze�"���W��J�j�����%���D`I]��ɸ��S�c+wd���6(ϣm�k����իW���Ó      �      x�U�ǲ��ҭ۪W!��]c7@+�>n���V�9qjwWhM3s�� ?B��.��A-><��j�Hϯ7
s����P
�js{/��'�F�J93,@��۝�[~X��H,��fb�̭o���k4Et�SA����ƨ"����T�r��r��E�{�֮���|�.�s�����	4�� �=1���y	�$��I���Iѿ��>�H�Ⱦ�K8�gI����S�=�H��S�.�q�`����5��]XI����^�,,����1`����gX)	wh��{�c]�����#*���?�0��,S��{���: �%V"c?(�L	/��L|��a�ѤaRv���2�_�c�#zG����P�63�#ɟ�p�[e�<�$�����,�8�ƺ��
R��Q��r�̣l7#U9s(�qe�7�v�(Sِw�=U`�F@��:G�^���i��p6��[��j{Kq�h�	/����\!ב��O'�'(�~�������W�{�h��?�A�)�H�����<O�f]�: /͛��Ñz���ٻ��ϣm��g�B�������c����4�F�DT7�[�f��~�%�m�ԯD�L(��%!���'�VZ��$}U�|��Q����P�Q1�ҥ���$��]}���*�� ����z[�3 af<+UL3��w�QlY;q�Û�l(�P:����{�e��.�1��m���K6�Y=`{^�K�p2H]!֟4�1�B������xg��m�W|�<�:}�he�����U�;���>����������W��xP�AjY�d5��噼��?=9���3(�ז ү��,�{k���ۓ��C�ِ�}j̸J�Ы���'�F�4j2��Ha�e�cL)���yL-�$s	��5��C�2{V�:�1�J`ĕ#�͕V���)�9]{���YE࿊�_�/�'�H���A�d�Y�c�d�$�W����c��&ry�%�pWY��"�X����D�LIRoӗ� ���e�%-������p���C�#�C��H8c�ƎH�wѢ�(Y�n&��\�����*.W@����[�?GA�.�5Ῠ�`��l���E��Ғ�P��g[=�豫e�q�E�Oh7�k?y��4}�gjD�@#]�~��n��z�2<�����/�i�R���k� ?K�L�-���j��	�J�[��_�'��o��g$v���N���鰂�'���t2�7�U��B�W�~@�2W%�kz=\%���n�%�RE���[��}d�P�V��ٵt�D(Ij���e���ë�;+�1�7�ݏ���A����J���[B�z�K6�j���G~,��|Ml�
�^�e� w r6@2�L0�C� e�ƹG��]���!��c�	���bI\/S�� Y?vm 9��_�L�M��y��hO���.�=�@�Ym��h�F��z!h�忑@��WǞ��C�l�ޖ�U�1�)�X�/V2��=��I� ��~�NqCJ�B�4��l�GKa@���u��LW	�_���?���#�:�M����4v��b9/���P"Ք*��p|��(� ^�6���G��'g�Z��.S�p��/ >_���sZ�[����@S|>�((�8`U	�C��~�O�@�����8~��z	f3����h�]�/�fW�o�݊W�tI}|��������	���F�bÂG_#�U�R�T.��5�Н��4Z��	��^���i�mGx��W �����C?�V��M��&V��S�%����u���������Nq���ݿ�w���7+�i�t�0l��	o�'{��\ �������>�=�@��?ڛ�ི;/}�t�2[#��qYg}�!�KC�	����[_y��G�<'��NZE��:�$���|q��&z�����?� \�!Y��G��Y�X�ح����히(�����w��c�+/�{Q�d�\����?5�ߠ��Ϗ`�a%����c�)�pI�x���eB�<�V�����6�������q�*`P�T�L����0ޣ+^��z�y��5�6#Ix
� ��ܳ��sv(]�h{�Jj�ȫ�F{�k�9��:�Z��Kn89����[D6��i������?��Z��C�t^֦�G��B�P���XS7+3�X+�}tpF��G]�4e%��+5���ү���E��Z-Y����Q>L�bW�.��W!r>y����Qxn��KbE��>��>��A����e����dȡ�������@��G���s�F4 ���hL�PȔ^L���jax�X��<f�y0YF�[��+Mr4e��'���m�y�}|�"���	��4_M�.�@�˅ң&�hLz'�)�i�/�Y@�O.�`/����"����:#�A�� �;�u�?=G>���h�ʾ�T8x ;��o��&ݷ���0S�ܻux_��7����&*�TS;����zt�d@!f�{plV�QF��.32�l��d������-����lq �Q�v�O�QFY��F��#�V�CF��g�3���h��l�c��o#�j�}2�C������EK�
��>�}����K�f������fB�	W����&fV�-�4l҈�/��5Y+���+��6�����.+��-K!@\�2O�Z���Ա�!������Lr%��,�eK����.����[ҟ���� c�Q����6�a���ib���&�s~��Nٓ����$,"��ϡ�aaxe+b��s�
g]��ڱ-q��a�Q�*�8��De�����53�|���h�Z묟�� �IOa���r�^��}O;�*��T%ȇ{MFS7.�\�`���f����<`���[��#[��)$�=g����
w�c����F�� �?k����F����(����0��8�F���o���2���� ��$���_���u���I�۲٩���M؍kf̗b�I)�0���@���n���qdRFg��!��u����m��A�=Z<��
ė ]+<Ft���K����&�B�tl���{ʡ�)���Y�S����+J�=��S�}+���a�Zi$�3߬����zgX{/���zw��zd���i�O�~Zv
)�K�n�������(ݚ~����{�?�����w=|��@���3����6d�,�����~�K�E�p��E_ ��'�$���ErT����_CsgƖ�L�e3�t��H�7nT���<���Ǧ+��ӫw��I!���x�Do�}n�E��]��;�!A��?�C���^ -���r�K����-{0^[�>}.�p��@�{3*��h�@�]frn����<���B8��
����`�D1�Φ���g~�jk�|�͒���pE�J��& ƣ�|��t��ٓ��wr�Uѿ� ��[�o���ʟ��r{�\�i�=9�z�'����M�*ˉ�(��d}�@ՠ�8�E����t�X��c鯙�.%u~�'�:,c�;�5��$O�`U�H�[�|��� V����eR3g���D�?S�j��D������_nԼ�:Zڐ� ��^��5�ep̾2�
�L�Σ��j��XxF�I�>3>�۝4Q�8��%wr7�f<��J!�����3j�0��e(eZ�9z�C?Lj*H��L�5_�Z�;�~��0DP2<�G���v?������WH��7o�0-���4���|E�\Jk��\E:������D�q�w�?�wZo|;��xZ5w<�]�������=�p��q��e%��Hﾀ���R"Yv�n�6"�e'oWhN�l���y�$���)���U�V��J��q����e����o�v���O�I�nB��u��/%��@|�d{���W�$M����guT�%^�-�	p�QK�L�2T
�U��OI׮��d�1�������S�˟�9U%�֒�v�9^�Xd�PeO�\0�h���H���=��0�=�q�?6��ܕ@ߗy�1�2^�s�y�l�+':B��mgB������������m������>ns�����    "ae���jNN��!�5����/u��=�ɝ��t����z+����^�o�J@��)fX e(�i��dEش��u����� ,�fgS�D]��V��A����5]2\a;U7�\,��]�Ӛ��q�w.0~?�W��W���M�r	
Dt���y���^D;���]UG�L�3Yop��G$�8�����Gm+�nr.&�gQy�|:^��\(I��{[���~o���]��Ư�m�G���o�6)����ݟ_w���-��]��)�|�'P.������?"�����x�h���X�x��#D� �_l^c�FE;�������?���}c�`G����wԕ4d�,P�{Z�C%<G.Q�zV��FjAq�_C����/������������[�AH��Ln�FU�V�,����6���jjؽ�#�ۮD�����d�x��wS�8ݢ��Xۨw�M�m��4Ȋ�}7w��\a��ˑ�NY��ao� %;�q���6�m��}}�I����^�&w�W����%�ۿ�:���Y�ZB���(i���w���Z2ڒ�rRM32 ~p��>Z�ѩ��N��ȫ'}i�j^GB���HzS����V���#���Y���U |�Rb#�i_��\N�B�Ўϩ�p�H�b�|)�Kd#l��{;t�Ϝ�~�������s�*��M�W��4P����]����T\n����b�������5V�w�}��FkS���0�뽕�<���w>�4�R��o%`T��Y`�QR�=��*�	��g�|Qq����l���*{�X-�t��~��~��P��?�W~���*S�?'��5y�'a���`�;&��zE7"�5K;<?��K�B��}�oi_"x�'�����(�W{C��[	��Q�u]\�+����Ch�|���g��	�9�Szw���0wz�mAl�X��W�ߊ�1�<��z��OE&��q�Rǰs���+��'h,�
E��ׄ�J�W]x��&y���%�Zԗ�&�b�Ɉ9�<-��桫��U���cv��t���L��h�Ss�S}�&O�`�����fU}VX�uq��f�(T��r~�3�7"�(�� ��ۺ\h�u��T�m(��R�8�o���ʫ��0^�[����ɟ"���+�~*Az!g"���z�D�������Nj}�s��O�Ϭ|��<��sQq^@&x�9��U�CK0h��bI��;��:�� �?� s��Ѓ��V�����Օ�W��xu#�����M�pA0�N�;��^Up!�g���m#��2�U�l�r��Y��*�>�gv��
Eڂ��ϛ�Ox9	��_�,Z���	����Ɗ/���T�+u���i�A.8,X��ʳ)�M=zS�+���|z���SE&��%�����#n��Q��7t�~���)���N�C+�h����@!j�-���VL:�7����"���T]�@&8�^2(�W~�,�J��ۛ�ڈ������,e��_|.3�r���/[�-̫Can������OX��Oы����3�P��ϟ-���d�`���P�yp��V��=J�J�+�����
���r�4O�T��n��í��a5 ΞO�vea�/km�-{}�7���s��&���y}�r����=�)-P�yw�~�S\!�8�I�f���2Ky1IHy#
�G�k"��.���p;폪t��<a	��y۔1���P����L{q[O�$d[[u8�l�v���B��C޷Ώ��K��>�U��V��e-�Y;���j%N"�3[�K�|�r�˶ ���~�� ̿[�5��� ���M�Şi�=ټ�AO���ró/#�5����B��l�������;�!P���m����UOH�Y�O�@Va�������'J�d�=�#��9H'Ll��	@�%�Y�޸�_T�����%���c6R��mhݮn���mI�
�nH>�;�4w�'h-��s�g� ��5AF�����n��<����?j~��tJ.�6�3�vB���*=4����N�;_2b/�Յ�쑿�E�"�2C�|���J(�TZZJ�VO�' ��)��3�l+p���O]ɲr;�E�<�Pt��O?�ʜ)|��:����r2� Ί�p�߬�WT%�?�psy^�� ��W���8o̢���'0�|�13`un��1��٬m^���,A?bD��o�#_.l����֗5�J��d��-�<��E���	�
`P���V���"O2a|-pMZy5�|Y;���ť(QlyI[����~����F�'�ޑ�F��*}yX��Ϗ	��헎L!���;�]L�P ��e6�K�.A�4|S?������o'�oi��9��ZS=��;�TiI�c�hV�W�]i��6�תq�0���|jX�LR�.҈y��9;�	�w�$�"����ԌeL7j��,�n����3
�q�|��놱<�[wo��+���=A�@�N�:��РO5�F����Zʯ�}��n�w���JTqѤnL�Љ������K�?���<%x��i��4������3���x8��Ԋ�7Ȏ�L�wV��6���L%�
�^�_� �q�l����u��O����-KE�B�=�^�lVrwN߮p�<��߬�T�}|a6�,05��@�u��9ۢ��ƗG�^
P�M�]Ob[�����x�p]�M�͒���Te 	�gqj����Q�>z��.j��)4��7���<�_Q���,޹�Nq��@ߟ���͢�J�o,U?��d�WI-~_�����(�s�\@��#�մ�!��ø9�����˨E}�&����a�MtW9$����FA��z���`�7(�I:�Si��9��|��-�w\E�$�_7�\:';k�F��C{W����:/:�Q�T?s`��]�zj��2�����+�T[%BtI��3h@�5Ŷ�k�^�9�#�X�E�#��	��Ç$�r�Ǌ���/!� �B�BSlSv<��v�GG�͍�/V�ψ&�EU�O�����*���3��+��y'�ZX��ڛ�thL���c���5����M�~���E�O�	����u}�]�X����������!Z��ө��
/��I��.0�q�����&&�Y��"�}�o���[}�R�Ϝ�*,������q����n�&�J�iꍷ�a�w��Ȫ����=� fK6ɐ��y�ϟ�q%�(��Ȭ8���g���p)�LσkA�F����'����!�S�{.ZQcv��&xN��.\�_�r����hg�x2]m�.���% �\�b�����WL����_7�g�E|�7D}`&;�������׵��Np���k�-|���~7=2H��j� ��^��*]kZQn T��_i �B"���Fb�b�������y��;�s(¬�Ĵ"��� ��7�Լ�OI'}����R��|���J�Iݿn�����=��':�L��ꊗ@��!�'~R]��-PNUj�~H�jg�:xu��f��fc�b�dW���<�`i=G)�{ﹳ�G�ұ;�b�e�u��%}l�;Ho:.w��d�d{��b�}�@��{Uv8�I�/�ɣ� �ET�O��usv�q���1-��'� IDן~�	����"��}��P�	r��T{s�Z�;�f��ib.����ך�B$;O-�>WC	*�I���0E�#T{�O���|�6,S#~58ӹSk�Q���>홞by��и�®��f��o!�GU�O��u����M]�V���Ο'�XDU��� 5�i��m!�r��6��G�d��F`B���vKa��p�4�9*������3''��Z��/l��̜�?
��1��<c��G��>S�z�ك�ysNս�/r����&5Fy��[���J�	߿nξ-��nf1��m(fJ��,��b�g>>P�����M3�60��-���	�;��QlyX��9ۦ�w���}���Յk��qU+o�~�ߙ]Nu���+�7Fm�Y�ӝ�K�u���p   xr3�^W멳�%gl���aP���C���w<��������wβ�l�z�A@ȚJ���r� \[|F;k�����������XĚ����]}��R����S���Q5�=���DX&V�ɹ��I�l��fň�W��O=�����:?��f�ڨKc�{6d��2�=7����K�㦿n�>�D`m�Y���(�����Mr���{^F��&#��_{+�O3:�|��9d�:O������o��S	�K3zܻyN���ɻ�Y�O��{Y˪��� ��O��ع4C�Ļ :R{�
~�<�d�d�-RM_ܔC�S��eⷂ������YXw"_��'�:s�ʐ����O;zN-~U=��P��q��5�/��w���hTI��<%�r��},[��S�]S��y�3�L�o"[���q�m����g�o�%�l!'�b�Q�"+ή.�NH� ,�)�I5��Ҽ��?����J�I�㨿n���f5�3ϻǊ<�lTt�x5	"o�N��ӹ�"����2jJз�����dT���\�! �&Gi��;���o׾j�1 F�8%�]0�EO����@�	=�i����Ǣk�;0��Pn��4_T��^�������#���G��X��?���; �D ������\Ք����-��`��N��I��6�aVT���NdCAv,|�=�ލ�P�m�m���f� �+	楄`FeY��Sx��Ҏ[��D�L���$G�N(�_Y`�����6J�H�6h\s��~PE�����q�_�������� �p      �     x�uT�n�P]���@Pl_�ɚ"�V]��RC"Ѥ�c�]��	��@����K��&"M�����3s?�va�3s�uf�Z�I��[o�i�>���Z{����۬��#+��A����σ�ͽ�n��f�UP�ԛ����z;��Y/��Z�[�Ѝ����jT�Kl�:��~�[���e�m�U:-���>��HT��ʪx�;����cyB_�q��C��c}���M��^��i����'Q/:�rY���K0vQ���( �,����"�@/�������&E�B�j����!dM�q�>��j&�I[	��#��4ŹB_f���`F���*��S�aԧx���=�ݟE�IW_���xHj6z\|����$=�x����p,�ĥ	Pq
Lڐ��/xPk�
�*�4��te&��ԅ��N� �1"�\E�\�uy�pC��N@���0���>��_�>h���A�9����n�*> <�,����1��`K�n�Kf�W�#T9@�S4y+��2$�-+����in�0����.}����#�T��\@��EVeZ��ڼܢ�ҘijS�Zt<��M\�7��x�aW�� ���4��@;��i��х�a�>����Gwzw}�b��咖�e�K�[N��_�=~m���J����7t6��z+�c�����L,�X��/j��Ό8�s��6�l�B�X>-�!S��J�CQ�h�\�������O
H���\ݝ_��£��V.�������U�D�{�|���%#޳�_%�&���|(�2C���R����^�      �     x�ՒG��H ��*DNi�x� Ja���AaO?�1}���2#��ֽ� &<e�^��ir6��OǮ�I�8›��Ou��6�$�Zۓ�9Qo{�%�b�7�E5ժv���.ؠ������ ���۴��8��59��`��*�ov�m�-u�n����*�|�)�SwZPw�I��.���б���FX~���K6v3�<�z.JM��r� ?69$����ucP����� ��N�����V�6��A}��#��2P}���,0��c�G��^R�"MOUuSxh�q]XⶃWǊ]Ym���s�b�|���Qﱯ�J��v?�MF��ͭZY��k�`D��B�����a��j�0ŀaMLʦ��+Y�4/�)c"^j 	ّ�9z�&�����W8"7�	q'�Q|�7뉞��Kr��ľV�?GRҺ���&c�{�Z�[b0ӕmF�m\d:z��Ga+_E{-)��>ni{T�GlM�[כw��SM�c�W�Ģ�� �w)�D�r��h�&�����;q���^�d����BǴ�F�|`?�v��ϛeơ4����G�6��$��]�>���&=s��Y1׽j�ln85H8?�s{bbY��U��s${�%A<��e�sX�e�$���t����+����lB���U�b`�k�������QzmR�<��W�����WX��(��q0��F�����33iS�Dm�xH\4|��O�q�ZT���yӑ��c|�1H�JkL^M&.ŕp��}�狆�}=�l�F�o|�0e����d�	]����?���?\h��     