Feature: Get customer
    As an API consumer
    I want to retrieve a customer via GET /customers/{id}
    So that I can look up a specific customer by their identifier

Scenario: Get an existing customer by id
    Given a customer has already been created
    When the customer is retrieved by id
    Then the response status is 200
    And the retrieved customer should match the created customer

Scenario: Get a non-existent customer
    When a non-existent customer id is requested
    Then the response status is 404
