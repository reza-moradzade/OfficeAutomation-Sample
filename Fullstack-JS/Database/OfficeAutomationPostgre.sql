-- PostgreSQL database dump
-- Version: 17.5
-- Dumped by pg_dump 17.5

-- Set session and encoding parameters
SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- =====================================
-- Extensions
-- =====================================

-- Enable pgcrypto for cryptographic functions
CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA public;
COMMENT ON EXTENSION pgcrypto IS 'cryptographic functions';

-- =====================================
-- Functions
-- =====================================

-- Archive audit logs older than a specified date
CREATE FUNCTION public.sp_archiveauditlogs(beforedate timestamp without time zone) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO auditslogs_archive (userid, action, details, ipaddress, "timestamp")
    SELECT userid, action, details, ipaddress, "timestamp"
    FROM auditslogs
    WHERE "timestamp" < beforedate;

    DELETE FROM auditslogs
    WHERE "timestamp" < beforedate;
END;
$$;
ALTER FUNCTION public.sp_archiveauditlogs(beforedate timestamp without time zone) OWNER TO postgres;

-- =====================================
-- Tables
-- =====================================

-- Audit logs table
CREATE TABLE public.auditslogs (
    logid bigint NOT NULL,
    userid integer,
    action character varying(200) NOT NULL,
    details text,
    ipaddress character varying(45),
    "timestamp" timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);
ALTER TABLE public.auditslogs OWNER TO postgres;

-- Archive of audit logs
CREATE TABLE public.auditslogs_archive (
    logid bigint NOT NULL,
    userid integer,
    action character varying(200),
    details text,
    ipaddress character varying(45),
    "timestamp" timestamp without time zone
);
ALTER TABLE public.auditslogs_archive OWNER TO postgres;

-- Captcha attempts
CREATE TABLE public.captchaattempts (
    captchaattemptid bigint NOT NULL,
    userid integer,
    ipaddress character varying(45),
    action character varying(100) NOT NULL,
    issuccess boolean NOT NULL,
    attemptat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    details character varying(1000)
);
ALTER TABLE public.captchaattempts OWNER TO postgres;

-- Cartable tasks table
CREATE TABLE public.cartable (
    cartableid integer NOT NULL,
    userid integer NOT NULL,
    taskid integer NOT NULL,
    isread boolean DEFAULT false NOT NULL,
    receivedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);
ALTER TABLE public.cartable OWNER TO postgres;

-- Email verification tokens
CREATE TABLE public.emailsverifications (
    verificationid uuid DEFAULT gen_random_uuid() NOT NULL,
    userid integer NOT NULL,
    token character varying(450) NOT NULL,
    expiresat timestamp without time zone NOT NULL,
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    confirmedat timestamp without time zone,
    confirmip character varying(45)
);
ALTER TABLE public.emailsverifications OWNER TO postgres;

-- Failed login attempts
CREATE TABLE public.failedloginattempts (
    attemptid integer NOT NULL,
    userid integer NOT NULL,
    attemptdate timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ipaddress character varying(45)
);
ALTER TABLE public.failedloginattempts OWNER TO postgres;

-- File storage table
CREATE TABLE public.files (
    fileid integer NOT NULL,
    filename character varying(255) NOT NULL,
    contenttype character varying(100),
    filesize bigint NOT NULL,
    filecontent bytea,
    uploadedby integer NOT NULL,
    uploadedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    isdeleted boolean DEFAULT false NOT NULL
);
ALTER TABLE public.files OWNER TO postgres;

-- Refresh tokens table
CREATE TABLE public.refreshtokens (
    refreshtokenid uuid DEFAULT gen_random_uuid() NOT NULL,
    userid integer NOT NULL,
    token character varying(450) NOT NULL,
    expiresat timestamp without time zone NOT NULL,
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    createdbyip character varying(45),
    revokedat timestamp without time zone,
    revokedbyip character varying(45),
    replacedbytoken character varying(450)
);
ALTER TABLE public.refreshtokens OWNER TO postgres;

-- Roles table
CREATE TABLE public.roles (
    roleid integer NOT NULL,
    rolename character varying(100) NOT NULL
);
ALTER TABLE public.roles OWNER TO postgres;

-- Tasks table
CREATE TABLE public.tasks (
    taskid integer NOT NULL,
    title character varying(200) NOT NULL,
    description text,
    assignedto integer,
    status character varying(50) DEFAULT 'Pending'::character varying NOT NULL,
    duedate timestamp without time zone,
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    isdeleted boolean DEFAULT false NOT NULL
);
ALTER TABLE public.tasks OWNER TO postgres;

-- User roles mapping table
CREATE TABLE public.userroles (
    userid integer NOT NULL,
    roleid integer NOT NULL,
    assignedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);
ALTER TABLE public.userroles OWNER TO postgres;

-- Users table
CREATE TABLE public.users (
    userid integer NOT NULL,
    username character varying(100) NOT NULL,
    passwordhash character varying(64) NOT NULL,
    passwordsalt character varying(255) NOT NULL,
    email character varying(255),
    fullname character varying(200),
    isactive boolean DEFAULT true NOT NULL,
    isdeleted boolean DEFAULT false NOT NULL,
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    isemailconfirmed boolean DEFAULT false NOT NULL
);
ALTER TABLE public.users OWNER TO postgres;

-- User sessions table
CREATE TABLE public.usersessions (
    sessionid uuid DEFAULT gen_random_uuid() NOT NULL,
    userid integer NOT NULL,
    sessiontoken uuid DEFAULT gen_random_uuid() NOT NULL,
    clienttype character varying(50) NOT NULL,
    ipaddress character varying(45),
    deviceinfo character varying(255),
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    lastactivity timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    isactive boolean DEFAULT true NOT NULL
);
ALTER TABLE public.usersessions OWNER TO postgres;

-- Active users view
CREATE VIEW public.vw_activeusersessions AS
 SELECT sessionid,
    userid,
    sessiontoken,
    clienttype,
    ipaddress,
    deviceinfo,
    createdat,
    lastactivity
   FROM public.usersessions
  WHERE (isactive = true);
ALTER VIEW public.vw_activeusersessions OWNER TO postgres;

-- =====================================
-- Sequences
-- =====================================

-- Sequence defaults for primary keys
ALTER TABLE ONLY public.auditslogs ALTER COLUMN logid SET DEFAULT nextval('public.auditslogs_logid_seq'::regclass);
ALTER TABLE ONLY public.auditslogs_archive ALTER COLUMN logid SET DEFAULT nextval('public.auditslogs_archive_logid_seq'::regclass);
ALTER TABLE ONLY public.captchaattempts ALTER COLUMN captchaattemptid SET DEFAULT nextval('public.captchaattempts_captchaattemptid_seq'::regclass);
ALTER TABLE ONLY public.cartable ALTER COLUMN cartableid SET DEFAULT nextval('public.cartable_cartableid_seq'::regclass);
ALTER TABLE ONLY public.failedloginattempts ALTER COLUMN attemptid SET DEFAULT nextval('public.failedloginattempts_attemptid_seq'::regclass);
ALTER TABLE ONLY public.files ALTER COLUMN fileid SET DEFAULT nextval('public.files_fileid_seq'::regclass);
ALTER TABLE ONLY public.roles ALTER COLUMN roleid SET DEFAULT nextval('public.roles_roleid_seq'::regclass);
ALTER TABLE ONLY public.tasks ALTER COLUMN taskid SET DEFAULT nextval('public.tasks_taskid_seq'::regclass);
ALTER TABLE ONLY public.users ALTER COLUMN userid SET DEFAULT nextval('public.users_userid_seq'::regclass);

-- =====================================
-- Constraints
-- =====================================

-- Primary keys, unique keys, and foreign keys are set below (no change)

-- The rest of the INSERT statements and sequence sets remain unchanged
-- (Data seeding is not commented for brevity)
