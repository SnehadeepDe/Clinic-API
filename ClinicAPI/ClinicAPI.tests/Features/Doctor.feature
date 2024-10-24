Feature: Doctors Resource

  Scenario Outline: Find doctors for a specific specialization
    Given I am a client
    When I make GET Request '/doctors?specialization=<specializationId>'
    Then response code must be '<statusCode>'
    And response data must look like '<expectedResponse>'

    Examples:
      | specializationId | statusCode | expectedResponse                  |
      | 1                | 200        | '[{"id": 1, "name": "Dr. Name", "specialization": "Orthopaedics", "phone": "1234567890", "email": "name@example.com", "experienceYears": 10}]' |
      | 999              | 404        | '{"message": "Specialization not found"}' |

  Scenario Outline: Get all schedules of a doctor
    Given I am a client
    When I make GET Request '/doctors/<doctorId>/availability'
    Then response code must be '<statusCode>'
    And response data must look like '<expectedResponse>'

    Examples:
      | doctorId | statusCode | expectedResponse                |
      | 1        | 200        | '[{"id": 1, "dayInWeek": "Monday", "startTime": "09:00 AM", "endTime": "05:00 PM"}]' |
      | 999      | 404        | '{"message": "Doctor not found"}' |
