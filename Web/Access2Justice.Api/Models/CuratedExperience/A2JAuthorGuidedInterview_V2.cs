using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Models.CuratedExperience
{
    public class A2JAuthorGuidedInterview_V2
    {
        public int authorId { get; set; }
        public string tool { get; set; }
        public string toolversion { get; set; }
        public string avatar { get; set; }
        public string avatarSkinTone { get; set; }
        public string avatarHairColor { get; set; }
        public string guideGender { get; set; }
        public string completionTime { get; set; }
        public string copyrights { get; set; }
        public string createdate { get; set; }
        public string credits { get; set; }
        public string description { get; set; }
        public string emailContact { get; set; }
        public string jurisdiction { get; set; }
        public string language { get; set; }
        public string modifydate { get; set; }
        public string notes { get; set; }
        public bool sendfeedback { get; set; }
        public string subjectarea { get; set; }
        public string title { get; set; }
        public string version { get; set; }
        public string viewer { get; set; }
        public string endImage { get; set; }
        public string logoImage { get; set; }
        public List<Author> authors { get; set; }
        public string firstPage { get; set; }
        public string exitPage { get; set; }
        public List<Step> steps { get; set; }
        public Vars vars { get; set; }
        public Pages pages { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public string title { get; set; }
        public string organization { get; set; }
        public string email { get; set; }
    }

    public class Step
    {
        public string number { get; set; }
        public string text { get; set; }
    }

    public class UserGender
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class UserAvatar
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class ClientFirstNameTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class ClientMiddleNameTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class ClientLastNameTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jVersion
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jInterviewId
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jBookmark
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jHistory
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jNavigationTf
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jInterviewIncompleteTf
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep0
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep1
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep2
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep3
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep4
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep5
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep6
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep7
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep8
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep9
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep10
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep11
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class AddressCityTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class AddressStreetTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class AddressZipcodeTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class ChildNameFirstTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class ClientNameFullTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class PhoneNumberTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class PurposeOfFormTe
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class NumberOfChildrenNu
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class ChildDobDa
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class TodayDateDa
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class HaveChildrenTf
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class AddressStateMc
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class MaritalStatusMc
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class ChildInformationFilter
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class Childcount
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class A2jStep12
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool repeating { get; set; }
        public string comment { get; set; }
    }

    public class Vars
    {
        public string UserGender { get; set; }
        public string UserAvatar { get; set; }
        public string ClientFirstNameTe { get; set; }
        public string ClientMiddleNameTe { get; set; }
        public string ClientLastNameTe { get; set; }
        public string A2jVersion { get; set; }
        public string A2jInterviewId { get; set; }
        public string A2jBookmark { get; set; }
        public string A2jHistory { get; set; }
        public string A2jNavigationTf { get; set; }
        public string A2jInterviewIncompleteTf { get; set; }
        public string A2jStep0 { get; set; }
        public string A2jStep1 { get; set; }
        public string A2jStep2 { get; set; }
        public string A2jStep3 { get; set; }
        public string A2jStep4 { get; set; }
        public string A2jStep5 { get; set; }
        public string A2jStep6 { get; set; }
        public string A2jStep7 { get; set; }
        public string A2jStep8 { get; set; }
        public string A2jStep9 /*__invalid_name__a2j step 9*/ { get; set; }
        public string A2jStep10 /*__invalid_name__a2j step 10*/ { get; set; }
        public string A2jStep11 { get; set; }
        public string AddressCityTe { get; set; }
        public string AddressStreetTe { get; set; }
        public string AddressZipcodeTe { get; set; }
        public string ChildNameFirstTe { get; set; }
        public string ClientNameFullTe { get; set; }
        public string PhoneNumberTe { get; set; }
        public string PurposeOfFormTe { get; set; }
        public string NumberOfChildrenNu { get; set; }
        public string ChildDobDa { get; set; }
        public string TodayDateDa { get; set; }
        public string HaveChildrenTf { get; set; }
        public string AddressStateMc { get; set; }
        public string MaritalStatusMc { get; set; }
        public string ChildInformationFilter { get; set; }
        public string Childcount { get; set; }
        public string A2jStep12 { get; set; }
    }

    public class Button
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Introduction
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button> buttons { get; set; }
        public List<object> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button2
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class Name
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button2> buttons { get; set; }
        public List<Field> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button3
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field2
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class Gender
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button3> buttons { get; set; }
        public List<Field2> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button4
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field3
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class Address
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button4> buttons { get; set; }
        public List<Field3> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button5
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field4
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class PhoneNumber
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button5> buttons { get; set; }
        public List<Field4> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button6
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field5
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class MaritalStatus
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button6> buttons { get; set; }
        public List<Field5> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button7
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class HaveChildren
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button7> buttons { get; set; }
        public List<object> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button8
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field6
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class HowManyChildren
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button8> buttons { get; set; }
        public List<Field6> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button9
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class TheEnd
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button9> buttons { get; set; }
        public List<object> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button10
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field7
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class PurposeOfForm
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button10> buttons { get; set; }
        public List<Field7> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Button11
    {
        public string label { get; set; }
        public string next { get; set; }
        public string url { get; set; }
        public string repeatVar { get; set; }
        public string repeatVarSet { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Field8
    {
        public string type { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string order { get; set; }
        public bool required { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public bool calculator { get; set; }
        public string maxChars { get; set; }
        public string listSrc { get; set; }
        public string listData { get; set; }
        public string sample { get; set; }
        public string invalidPrompt { get; set; }
    }

    public class ChildsNameAndBirthdate
    {
        public string name { get; set; }
        public string type { get; set; }
        public int step { get; set; }
        public string repeatVar { get; set; }
        public bool nested { get; set; }
        public string outerLoopVar { get; set; }
        public string text { get; set; }
        public string textCitation { get; set; }
        public string textAudioURL { get; set; }
        public string learn { get; set; }
        public string help { get; set; }
        public string helpCitation { get; set; }
        public string helpAudioURL { get; set; }
        public string helpReader { get; set; }
        public string helpImageURL { get; set; }
        public string helpVideoURL { get; set; }
        public List<Button11> buttons { get; set; }
        public List<Field8> fields { get; set; }
        public string codeBefore { get; set; }
        public string codeAfter { get; set; }
        public string codeCitation { get; set; }
        public string notes { get; set; }
    }

    public class Pages
    {
        public string Introduction { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MaritalStatus { get; set; }
        public string HaveChildren { get; set; }
        public string HowManyChildren { get; set; }
        public string TheEnd { get; set; }
        public string PurposeOfForm { get; set; }
        public string ChildsNameAndBirthdate { get; set; }
    }
}