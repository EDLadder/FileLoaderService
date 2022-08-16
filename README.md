# FileLoaderService

## Should develop a backend service using .Net core 6. The Service should:

- [x] Every 1 minute service connects to sftp and checks if there are new files.
- [x] Sftp server, file paths etc. must be configurable (not in code).
- [x] Service downloads all the new files to local path.
- [x] All downloaded files (paths) should be stored in database (postgresql).
- [x] Files from sftp are never deleted, so checking if file is new or old has to be done by checking it in database taken in consideration file creation time.
- [x] Work with database should by done by Entity framework.
- [x] Database should be defined by code first principle.
- [x] Service should be resilient: handle connection problems etc. and should not "die".
- [x] Code must have comments explaining what it does.
- [x] Service should have sane logging, configurable tracing (it should be clear what is happening from logs).
- [x] Service should use dependency injection.
