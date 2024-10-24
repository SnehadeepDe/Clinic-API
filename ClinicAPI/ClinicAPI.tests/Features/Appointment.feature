Feature: Appointments Resource

  Scenario Outline: Create a new appointment for the patient
    Given I am a client
    When I make POST Request '/appointments' with the following Data '<postDataJson>'
    Then response code must be '<statusCode>'
    And response data must look like '<expectedResponse>'

    Examples:
      | postDataJson                                                                                      | statusCode | expectedResponse                   |
      | {"patientId": 1, "doctorId": 1, "appointmentDate": "2024-07-15", "appointmentTime": "10:00 AM"}   | 201        | '{"appointmentId": 1}'             |
      | {"patientId": 999, "doctorId": 1, "appointmentDate": "2024-07-15", "appointmentTime": "10:00 AM"} | 404        | '{"message": "Patient not found"}' |

  Scenario Outline: Update Appointment Time
    Given I am a client
    When I make PATCH Request '/api/appointments/<appointmentId>' with the following Data '{"appointmentTime":"<newAppointmentTime>"}'
    Then response code must be '<statusCode>'
    And response data must look like '<expectedResponse>'

    Examples:
      | appointmentId | newAppointmentTime | statusCode | expectedResponse                       |
      | 1             | "11:00 AM"         | 200        | '{}'                                   |
      | 999           | "12:00 PM"         | 404        | '{"message": "Appointment not found"}' |

 Scenario Outline: Fetch Appointments by Patient Id
    Given I am a client
    When I make GET Request '/appointments?patientid=<patientId>'
    Then response code must be '<statusCode>'
    And response data must look like '<expectedResponse>'

    Examples:
      | patientId | statusCode | expectedResponse                                                                                                                          |
      | 1         | 200        | '[{"id": 1, "doctorName": "Doctor Name", "patientName": "Patient Name", "appointmentDate": "2024-07-15", "appointmentTime": "10:00 AM"}]' |
      | 999       | 404        | '{"message": "Patient not found"}'                                                                                                        |