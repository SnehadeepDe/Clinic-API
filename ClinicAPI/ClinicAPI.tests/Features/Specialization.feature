Feature: Specializations Resource

  Scenario: Get All Specializations
    Given I am a client
    When I make GET Request '/specializations'
    Then response code must be '200'
    And response data must look like '[{"specializationId": 1, "specializationName": "Orthopaedics"}, {"specializationId": 2, "specializationName": "Pediatrics"}]'

  Scenario Outline: Create a New Specialization
    Given I am a client
    When I make POST Request '/specializations' with the following Data '<postDataJson>'
    Then response code must be '<statusCode>'
    And response data must look like '<expectedResponse>'

    Examples:
      | postDataJson                          | statusCode | expectedResponse                   |
      | {"specializationName": "Dermatology"} | 201        | '{"id": 3}'                        |
      | {"specializationName": ""}            | 400        | '{"message": "Name is required."}' |
