# Project Tests

This document lists the automated tests for various components of the project.

## PersonServiceTests (43 Tests)

This section lists the automated tests for the `PersonService`. All tests are currently passing.

* `CreatePerson_InvalidPerson_ShouldHaveError`
* `CreatePerson_NullPerson_ShouldThrowArgumentNullException`
* `CreatePerson_RepositoryThrowsException_ShouldPropagate`
* `CreatePerson_ValidPerson_ShouldCallAddAndReturnTrue`
* `DeletePerson_WhenPersonDoesNotExist_ShouldReturnFalse`
* `DeletePerson_WhenPersonExists_ShouldReturnTrue`
* `ExportToExcel_ShouldReturnValidExcelFile`
* `FilterByBirthYear_DefaultChoice_ShouldReturnAll`
* `FilterByBirthYear_EqualChoice_ShouldReturnMatchingYear`
* `FilterByBirthYear_GreaterChoice_ShouldReturnMatchingYear`
* `FilterByBirthYear_InvalidYear_ShouldThrowArgumentOutOfRangeException`
* `FilterByBirthYear_LessChoice_ShouldReturnMatchingYear`
* `FilterByBirthYear_NullChoice_ShouldReturnAll`
* `FilterByBirthYear_WithVariousChoices_ReturnsExpectedCount`
* `GetAllPeople_WhenDataExists_ShouldReturnAllPeople`
* `GetAllPeople_WhenNoData_ShouldReturnEmptyList`
* `GetAllPeople_WhenRepositoryReturnsNull_ShouldReturnEmptyList`
* `GetFullName_WhenPersonDoesNotExist_ShouldReturnNull`
* `GetFullName_WhenPersonExists_ShouldReturnFullName`
* `GetFullNames_WhenNoPersonsExist_ShouldReturnEmptyList`
* `GetFullNames_WhenPersonsExist_ShouldReturnFullNames`
* `GetMales_WhenMalesExist_ShouldReturnOnlyMales`
* `GetMales_WhenNoMalesExist_ShouldReturnEmptyList`
* `GetMales_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList`
* `GetOldestPerson_WhenNoPeopleExist_ShouldReturnNull`
* `GetOldestPerson_WhenPeopleExist_ShouldReturnOldest`
* `GetPagedPersons_PageOutOfRange_ShouldReturnEmptyList`
* `GetPagedPersons_ShouldReturnCorrectPage1`
* `GetPagedPersons_ShouldReturnCorrectPage2`
* `GetPerson_WhenPersonDoesNotExist_ShouldReturnNull`
* `GetPerson_WhenPersonExists_ShouldReturnPerson`
* `UpdatePerson_InvalidId_ShouldReturnFalse`
* `UpdatePerson_InvalidPerson_ShouldHaveError`
* `UpdatePerson_NullPerson_ShouldThrowArgumentNullException`
* `UpdatePerson_RepositoryThrows_ShouldPropagate`
* `UpdatePerson_ShouldSetCorrectIdBeforeUpdate`
* `UpdatePerson_ValidPerson_ShouldCallUpdateAndReturnTrue`

## PersonControllerTest (27 Tests)

This section lists the automated tests for the `PersonController`. All tests are currently passing.

* `AddAPerson_Get_ReturnsView`
* `AddAPerson_Post_ValidPerson_CreatesPersonAndRedirects`
* `AddAPerson_Post_WhenException_ReturnsErrorMessageAndRedirects`
* `DeleteAPerson_ExceptionThrown_ReturnsRedirectToIndexWithError`
* `DeleteAPerson_ValidId_DeletesAndRedirects`
* `EditAPerson_Get_ExistingId_ReturnsViewWithPerson`
* `EditAPerson_Get_NonExistingId_ReturnsNotFound`
* `EditAPerson_Post_ThrowsException_ReturnsRedirectToIndex`
* `EditAPerson_Post_UpdateReturnsFalse_ReturnsNotFound`
* `EditAPerson_Post_ValidUpdate_ReturnsRedirectToIndex`
* `ExportToExcel_ReturnsFile_WhenServiceSucceeds`
* `ExportToExcel_ThrowsException_ReturnsBadRequestWithTempDataMessage`
* `FilterByBirthYear_WhenChoiceIsAfter_ReturnsPersonsBornAfterYear`
* `FilterByBirthYear_WhenChoiceIsBefore_ReturnsPersonsBornBeforeYear`
* `FilterByBirthYear_WhenChoiceIsEquals_ReturnsPersonsBornInYear`
* `FilterByBirthYear_WhenChoiceIsInvalid_ReturnsEmptyList`
* `FilterByBirthYear_WhenNoPersonMatches_ReturnsEmptyList`
* `FullNames_ReturnsViewWithListOfFullNames`
* `FullNames_WhenNoPersons_ReturnsEmptyList`
* `Index_ReturnsViewWithPagedResult`
* `Index_UsesDefaultPageValues_WhenNoParametersProvided`
* `Males_ReturnsViewWithMalePersons`
* `Males_WhenNoMalePersons_ReturnsEmptyList`
* `Oldest_ReturnsViewWithOldestPerson`
* `Oldest_WhenNoPersonExists_ReturnsNullModel`
* `ViewAPerson_ExistingId_ReturnsViewWithPerson`
* `ViewAPerson_NonExistingId_ReturnsNotFound`