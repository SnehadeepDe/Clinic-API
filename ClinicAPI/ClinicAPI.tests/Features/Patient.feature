Feature: Patients Resource

  Scenario Outline: Create a new patient
    Given I am a client
    When I make POST Request '/patients' with the following Data '<postDataJson>'
    Then response code must be '<statusCode>'
    And response data must look like '<expectedResponse>'

    Examples:
      | postDataJson                                                                    | statusCode | expectedResponse                  |
      | {"name": "Patient name", "phone": "9876543210", "email": "patient@example.com"} | 201        | '{"id": 1}'                       |
      | {"phone": "9876543210", "email": "patient@example.com"}                         | 400        | '{"message": "Name is required"}' |


