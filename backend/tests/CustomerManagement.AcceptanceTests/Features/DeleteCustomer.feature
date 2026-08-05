Feature: Delete customer
    As an API consumer
    I want to delete a customer via DELETE /customers/{id}
    So that customers can be removed and are no longer retrievable

Scenario: Delete an existing customer
    Given a customer has already been created
    When the customer is deleted by id
    Then the response status is 204

Scenario: Delete a non-existent customer
    When a non-existent customer id is deleted
    Then the response status is 404

Scenario: A deleted customer cannot be retrieved afterwards
    Given a customer has already been created
    When the customer is deleted by id
    Then the response status is 204
    When the deleted customer is retrieved by id
    Then the response status is 404
