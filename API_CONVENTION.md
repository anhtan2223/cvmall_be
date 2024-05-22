# Request
### Path
- Path using snake_case : `master-code` => `master_code`
  ```cs
  GET: api/master_code/get_list // Get list
  GET: api/master_code/get_by_id/{id} // Get detail by id
  POST: api/master_code/create  // Create new
  PUT: api/master_code/update/{id}  // Update
  DELETE: api/master_code/delete_by_id/{id}  // Delete by id
  DELETE: api/master_code/delete_multi  // Delete by list id
  ```
### Method
- `GET` : api get data, not change data info
- `POST` : create new record in database
- `PUT` : update data info
- `DELETE` : api delete action
### Paging
- Query `{{domain}}/api/master_code?page=1&size=100&sort=code.asc&search=abc`
  ```json
  {
    "page": 1, // default 1
    "size": 100, // default 10
    "search": "",
    "sort": ""
  }
  ```

### Sort
- Sort string = field_name`.`sort type 
- Only sort 1 field at a time \
EX: `sort=code.asc` or `sort=code.desc`

# Response

### Paging
```json
{
    "page": 1,
    "size": 100,
    "total": 3,
    "data": [
        {
            "id": "7bd35008-ce89-45ec-8674-9609baa3805a",
            "type": "GENDER",
            "key": "0",
            "value": "他の"
        },
        {
            "id": "87e9c579-e701-4a0a-a2e5-21e3c47b515e",
            "type": "GENDER",
            "key": "2",
            "value": "女性"
        },
    ]
}
```

### Validate error
```json
{
    "code": 1,
    "message": "リクエストコンテンツが無効です",
    "errors": [
        {
            "name": "key",
            "code": 0,
            "message": "key is required"
        }
    ],
    "data": null
}
```

### Create, update success
```json
{
    "code": 0,
    "message": "コードの成功を作成する"
}
```

### Response code

```json
{
    "Success": 0,
    "SystemError": 1,
    "NotFound": 2,
    "Invalid": 3,
    "UnAuthorized": 4
}
```


