using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Models.CuratedExperienceV1
{
    public class A2JAuthorGuidedInterview
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
        //            public UserGender __invalid_name__user gender { get; set; }
        //        public UserAvatar __invalid_name__user avatar { get; set; }
        //    public ClientFirstNameTe __invalid_name__client first name te { get; set; }
        //public ClientMiddleNameTe __invalid_name__client middle name te { get; set; }
        //    public ClientLastNameTe __invalid_name__client last name te { get; set; }
        //    public A2jVersion __invalid_name__a2j version { get; set; }
        //    public A2jInterviewId __invalid_name__a2j interview id { get; set; }
        //public A2jBookmark __invalid_name__a2j bookmark { get; set; }
        //    public A2jHistory __invalid_name__a2j history { get; set; }
        //    public A2jNavigationTf __invalid_name__a2j navigation tf { get; set; }
        //public A2jInterviewIncompleteTf __invalid_name__a2j interview incomplete tf { get; set; }
        //    public A2jStep0 __invalid_name__a2j step 0 { get; set; }
        //    public A2jStep1 __invalid_name__a2j step 1 { get; set; }
        //    public A2jStep2 __invalid_name__a2j step 2 { get; set; }
        //    public A2jStep3 __invalid_name__a2j step 3 { get; set; }
        //    public A2jStep4 __invalid_name__a2j step 4 { get; set; }
        //    public A2jStep5 __invalid_name__a2j step 5 { get; set; }
        //    public A2jStep6 __invalid_name__a2j step 6 { get; set; }
        //    public A2jStep7 __invalid_name__a2j step 7 { get; set; }
        //    public A2jStep8 __invalid_name__a2j step 8 { get; set; }
        //    public A2jStep9 __invalid_name__a2j step 9 { get; set; }
        //    public A2jStep10 __invalid_name__a2j step 10 { get; set; }
        //    public A2jStep11 __invalid_name__a2j step 11 { get; set; }
        //    public AddressCityTe __invalid_name__address city te { get; set; }
        //public AddressStreetTe __invalid_name__address street te { get; set; }
        //public AddressZipcodeTe __invalid_name__address zipcode te { get; set; }
        //public ChildNameFirstTe __invalid_name__child name first te { get; set; }
        //    public ClientNameFullTe __invalid_name__client name full te { get; set; }
        //    public PhoneNumberTe __invalid_name__phone number te { get; set; }
        //public PurposeOfFormTe __invalid_name__purpose of form te { get; set; }
        //    public NumberOfChildrenNu __invalid_name__number of children nu { get; set; }
        //    public ChildDobDa __invalid_name__child dob da { get; set; }
        //public TodayDateDa __invalid_name__today date da { get; set; }
        //public HaveChildrenTf __invalid_name__have children tf { get; set; }
        //public AddressStateMc __invalid_name__address state mc { get; set; }
        //public MaritalStatusMc __invalid_name__marital status mc { get; set; }
        //public ChildInformationFilter __invalid_name__child information filter { get; set; }
        //public Childcount childcount { get; set; }
        //public A2jStep12 __invalid_name__a2j step 12 { get; set; }
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

    public class __invalid_type__1Introduction
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

    public class __invalid_type__1Name
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

    public class __invalid_type__2Gender
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

    public class __invalid_type__3Address
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

    public class __invalid_type__4PhoneNumber
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

    public class __invalid_type__1MaritalStatus
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

    public class __invalid_type__2HaveChildren
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

    public class __invalid_type__3HowManyChildren
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

    public class __invalid_type__1TheEnd
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

    public class __invalid_type__1PurposeOfForm
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

    public class __invalid_type__4ChildSNameAndBirthdate
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
        //    public __invalid_type__1Introduction __invalid_name__1-Introduction { get; set; }
        //public __invalid_type__1Name __invalid_name__1-Name { get; set; }
        //    public __invalid_type__2Gender __invalid_name__2-Gender { get; set; }
        //    public __invalid_type__3Address __invalid_name__3-Address { get; set; }
        //    public __invalid_type__4PhoneNumber __invalid_name__4-Phone number { get; set; }
        //public __invalid_type__1MaritalStatus __invalid_name__1-Marital Status { get; set; }
        //public __invalid_type__2HaveChildren __invalid_name__2-Have children? { get; set; }
        //    public __invalid_type__3HowManyChildren __invalid_name__3-How many children? { get; set; }
        //    public __invalid_type__1TheEnd __invalid_name__1-The End { get; set; }
        //public __invalid_type__1PurposeOfForm __invalid_name__1-Purpose of form { get; set; }
        //    public __invalid_type__4ChildSNameAndBirthdate __invalid_name__4-Child's name and birthdate { get; set; }
    }
}
