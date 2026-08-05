Feature: Update customer
    As an API consumer
    I want to update a customer via PUT /customers/{id}
    So that customer details can be corrected or changed

Scenario: Update an existing customer with valid details
    Given a customer has already been created
    When the customer is updated with the following details
        | FirstName   | LastName         | Email                   |
        | Ada Updated | Lovelace Updated | ada.updated@example.com |
    Then the response status is 200
    And the updated customer should match the submitted details

Scenario: Update a non-existent customer
    When a non-existent customer id is updated with valid details
    Then the response status is 404

Scenario: Reject an update with a missing required field
    Given a customer has already been created
    When the customer is updated with the following details
        | FirstName | LastName | Email           |
        |           | Lovelace | ada@example.com |
    Then the response status is 400
