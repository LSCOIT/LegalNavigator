$(document).ready(function ()
{
    //-----------------------------------------------------------------------------------------
    //Populate Scenario Table
    $('#TableScenarioId').DataTable(
    {
        "columnDefs": [
            { "width": "5%", "targets": [0] },
            { "className": "text-center custom-middle-align", "targets": [0, 1, 2, 3] },
        ],
        "language":
            {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
        "processing": true,
        "serverSide": true,
        "ajax":
            {
            "url": "/Home/GetScenarioData",
            "type": "POST",
            "data": { migrationId: document.getElementById('migrationId').value},
             "dataType": "JSON"
            },
        "columns": [
            { "data": "ScenarioId" },
            { "data": "LC_ID" },
            { "data": "Description" },
            { "data": "Outcome" }                    
        ]
        });
 //=================================================================================================================
    //-----------------------------------------------------------------------------------------
    //Populate LawCategory Table
    $('#TableLawCategoryId').DataTable(
        {
            "columnDefs": [
                { "width": "5%", "targets": [0] },
                { "className": "text-center custom-middle-align", "targets": [0, 1, 2, 3] },
            ],
            "language":
            {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "processing": true,
            "serverSide": true,
            "ajax":
            {
                "url": "/Home/GetLawCategoryData",
                "type": "POST",
                "data": { migrationId: document.getElementById('migrationId').value },
                "dataType": "JSON"
            },
            "columns": [
                { "data": "LCID" },
                { "data": "NSMICode" },
                { "data": "Description" },
                { "data": "StateDeviation" }               
            ]
        });
 //=================================================================================================================
    //-----------------------------------------------------------------------------------------
    //Populate Process Table
    $('#TableProcessId').DataTable(
        {
            "columnDefs": [
                { "width": "5%", "targets": [0] },
                { "className": "text-center custom-middle-align", "targets": [0, 1, 2, 3, 4] },
            ],
            "language":
            {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "processing": true,
            "serverSide": true,
            "ajax":
            {
                "url": "/Home/GetProcessData",
                "type": "POST",
                "data": { migrationId: document.getElementById('migrationId').value },
                "dataType": "JSON"
            },
            "columns": [
                { "data": "Id" },
                { "data": "Title" },
                { "data": "Description" },
                { "data": "ActionJson" },
                { "data": "LC_ID" }
            ]
        });
 //=================================================================================================================
    //-----------------------------------------------------------------------------------------
    //Populate Scenario Table
    $('#TableResourceId').DataTable(
        {
            "columnDefs": [
                { "width": "5%", "targets": [0] },
                { "className": "text-center custom-middle-align", "targets": [0, 1, 2, 3, 4, 5] },
            ],
            "language":
            {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "processing": true,
            "serverSide": true,
            "ajax":
            {
                "url": "/Home/GetResourceData",
                "type": "POST",
                "data": { migrationId: document.getElementById('migrationId').value },
                "dataType": "JSON"
            },
            "columns": [
                { "data": "ResourceId" },
                { "data": "ResourceType" },
                { "data": "ResourceJson" },
                { "data": "Title" },
                { "data": "Action" },
                { "data": "LC_ID" }
            ]
        });
 //=================================================================================================================


    //----------Uploaded Video renderer----------
    //-----------------------------------------------------------------------------------------
    //Populate Video Table
    $('#VideoId').DataTable(
        {
            "columnDefs": [
                { "width": "5%", "targets": [0] },
                { "className": "text-center custom-middle-align", "targets": [0, 1, 2, 3,4,5] },
            ],
            "language":
            {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "processing": true,
            "serverSide": true,
            "ajax":
            {
                "url": "/Home/GetVideoData",
                "type": "POST",
                "data": { migrationId: document.getElementById('migrationId').value },
                "dataType": "JSON"
            },
            "columns": [
                { "data": "VideoId" },
                { "data": "Title" },
                { "data": "Url" },
                { "data": "ResourceJson" },
                { "data": "ActionType" },
                { "data": "LCID" }
            ]
        });
 //=================================================================================================================

    //=================================================================================================================


    //----------Uploaded QA renderer----------
    //-----------------------------------------------------------------------------------------
    //Populate QA Table
    $('#QAId').DataTable(
        {
            "columnDefs": [
                { "width": "5%", "targets": [0] },
                { "className": "text-center custom-middle-align", "targets": [0, 1, 2, 3, 4] },
            ],
            "language":
            {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "processing": true,
            "serverSide": true,
            "ajax":
            {
                "url": "/Home/GetQAData",
                "type": "POST",
                "data": { migrationId: document.getElementById('migrationId').value },
                "dataType": "JSON"
            },
            "columns": [
                { "data": "QAId" },
                { "data": "Question" },
                { "data": "Answer" },
                { "data": "NsmiCode" },
                { "data": "Intent" }
               
            ]
        });
 //=================================================================================================================

});
