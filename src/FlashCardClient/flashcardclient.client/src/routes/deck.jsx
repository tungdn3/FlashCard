import { Form as RRDForm, useLoaderData, useRevalidator } from "react-router-dom";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import FlashCard from "../components/FlashCard";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import axios from "axios";
import { useState } from "react";

export async function loader({ params }) {
  const response = await fetch(`/v1/decks/${params.deckId}/cards`);
  const cards = await response.json();
  return { cards, deckId: params.deckId };
}

export async function action({ request, params }) {
  const formData = await request.formData();
  const card = Object.fromEntries(formData);
  await axios.post(`/v1/decks/${params.deckId}/cards`, card);
  // todo: handle error
  return null;
}

export default function Deck() {
  const { cards, deckId } = useLoaderData();
  let revalidator = useRevalidator();

  const [showForm, setShowForm] = useState(false);

  async function handleDeleteCard(cardId) {
    await axios.delete(`/v1/decks/${deckId}/cards/${cardId}`);
    revalidator.revalidate();
  }

  return (
    <div className="mt-3">
      <div className="d-flex justify-content-end">
        <Button onClick={() => setShowForm(true)}>New Card</Button>
      </div>

      {cards && cards.length ? (
        <Row
          xs={1}
          md={2}
          lg={3}
          xxl={4}
          className="g-4"
          style={{ marginTop: "10rem", marginLeft: "1rem" }}
        >
          {cards.map((card) => (
            <Col key={card.id} style={{ minHeight: 300 }}>
              <FlashCard
                id={card.id}
                word={card.word}
                meaning={card.meaning}
                sentenses={["I like apple"]}
                onDelete={handleDeleteCard}
              />
            </Col>
          ))}
        </Row>
      ) : (
        <div> No cards</div>
      )}

      <Modal show={showForm} onHide={() => setShowForm(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Create Deck</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <RRDForm method="post">
            <Form.Group className="mb-3">
              <Form.Control name="word" placeholder="Word" autoFocus />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Control name="meaning" as="textarea" rows={2} placeholder="Meaning" />
            </Form.Group>
            {/* <Form.Group
              className="mb-3"
            >
              <Form.Label>Meaning</Form.Label>
              <Form.Control as="textarea" rows={3} />
            </Form.Group> */}
            <div className="d-flex">
              <Button
                variant="primary"
                type="submit"
                className="ms-auto"
                onClick={() => setShowForm(false)}
              >
                Create
              </Button>
            </div>
          </RRDForm>
        </Modal.Body>
      </Modal>
    </div>
  );
}
