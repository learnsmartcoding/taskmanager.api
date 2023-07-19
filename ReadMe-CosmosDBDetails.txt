Sample data
{
    "id": "task1",
    "taskId": null,
    "userId": "user123",
    "title": "Complete Project Proposal enclosed",
    "description": "Write and submit the project proposal by end of the week.",
    "dueDate": "2023-06-15T00:00:00",
    "status": "Incomplete",
    "priority": "High",
    "labels": [
        "Work",
        "Important"
    ],
    "attachments": [
        {
            "id": "attachment1",
            "fileName": "proposal.docx",
            "url": "https://taskmanagerstorage.blob.core.windows.net/attachments/user123/proposal.docx"
        },
        {
            "id": "attachment1",
            "fileName": "proposal.docx",
            "url": "https://taskmanagerstorage.blob.core.windows.net/attachments/user123/proposal.docx"
        }
    ],
    "subtasks": [
        {
            "id": "subtask1",
            "title": "Research",
            "status": "Done"
        },
        {
            "id": "subtask2",
            "title": "Write",
            "status": "Incomplete"
        }
    ],
    "_rid": "QkdTAPK14D8BAAAAAAAAAA==",
    "_self": "dbs/QkdTAA==/colls/QkdTAPK14D8=/docs/QkdTAPK14D8BAAAAAAAAAA==/",
    "_etag": "\"3000f3e5-0000-0700-0000-648ca6b00000\"",
    "_attachments": "attachments/",
    "_ts": 1686939312
}

{
    "id": "task2",
    "userId": "user456",
    "title": "Prepare Presentation",
    "description": "Create a PowerPoint presentation for the upcoming meeting.",
    "dueDate": "2023-06-20",
    "status": "Incomplete",
    "priority": "Medium",
    "labels": [
        "Work",
        "Meeting"
    ],
    "attachments": [
        {
            "id": "attachment2",
            "fileName": "presentation.pptx",
            "url": "https://taskmanagerstorage.blob.core.windows.net/attachments/user456/presentation.pptx"
        }
    ],
    "subtasks": [
        {
            "id": "subtask3",
            "title": "Gather data",
            "status": "Complete"
        },
        {
            "id": "subtask4",
            "title": "Create slides",
            "status": "Incomplete"
        },
        {
            "id": "subtask5",
            "title": "Practice delivery",
            "status": "Incomplete"
        }
    ],
    "_rid": "QkdTAPK14D8CAAAAAAAAAA==",
    "_self": "dbs/QkdTAA==/colls/QkdTAPK14D8=/docs/QkdTAPK14D8CAAAAAAAAAA==/",
    "_etag": "\"0000940e-0000-0700-0000-648b4bb30000\"",
    "_attachments": "attachments/",
    "_ts": 1686850483
}

{
    "id": "user123",
    "userId": "user123",
    "name": "John Doe",
    "email": "john.doe.one@example.com",
    "settings": {
        "timezone": "America/New_York",
        "notificationEnabled": true
    },
    "_rid": "QkdTAOHLCz8BAAAAAAAAAA==",
    "_self": "dbs/QkdTAA==/colls/QkdTAOHLCz8=/docs/QkdTAOHLCz8BAAAAAAAAAA==/",
    "_etag": "\"3000f0e5-0000-0700-0000-648ca5c60000\"",
    "_attachments": "attachments/",
    "_ts": 1686939078
}

{
    "id": "user456",
    "userId": "user456",
    "name": "Jane Smith",
    "email": "jane.smith@example.com",
    "settings": {
        "timezone": "Europe/London",
        "notificationEnabled": false
    },
    "_rid": "QkdTAOHLCz8CAAAAAAAAAA==",
    "_self": "dbs/QkdTAA==/colls/QkdTAOHLCz8=/docs/QkdTAOHLCz8CAAAAAAAAAA==/",
    "_etag": "\"00008b0e-0000-0700-0000-648b4ab60000\"",
    "_attachments": "attachments/",
    "_ts": 1686850230
}
{
    "id": "user789",
    "userId": "user789",
    "name": "Alex Johnson",
    "email": "alex.johnson@example.com",
    "settings": {
        "timezone": "Asia/Tokyo",
        "notificationEnabled": true
    },
    "_rid": "QkdTAOHLCz8DAAAAAAAAAA==",
    "_self": "dbs/QkdTAA==/colls/QkdTAOHLCz8=/docs/QkdTAOHLCz8DAAAAAAAAAA==/",
    "_etag": "\"00008c0e-0000-0700-0000-648b4ac90000\"",
    "_attachments": "attachments/",
    "_ts": 1686850249
}
