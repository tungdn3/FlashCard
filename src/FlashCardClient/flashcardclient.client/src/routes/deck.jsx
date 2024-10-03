import {
  json,
  Form as RRDForm,
  useLoaderData,
  useRevalidator,
} from "react-router-dom";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import FlashCard from "../components/FlashCard";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import axios from "axios";
import { useState } from "react";
import Learn from "../components/Learn";

export async function loader({ params }) {
  const response = await fetch(`/v1/decks/${params.deckId}/cards`);
  // todo: handle cookie expired properly
  if (response.status === 401) {
    window.location.href = "/auth/login";
  }
  const cards = await response.json();
  return { cards, deckId: params.deckId };
}

export async function action({ request, params }) {
  const formData = await request.formData();
  const card = Object.fromEntries(formData);
  const intent = formData.get("intent");

  if (intent === "create") {
    await axios.post(`/v1/decks/${params.deckId}/cards`, card);
    return { ok: true };
  }

  if (intent === "edit") {
    await axios.put(`/v1/decks/${params.deckId}/cards/${card.id}`, card);
    return { ok: true };
  }

  throw json({ message: "Invalid intent" }, { status: 400 });
}

export default function Deck() {
  const { cards, deckId } = useLoaderData();
  let revalidator = useRevalidator();

  const [showForm, setShowForm] = useState(false);
  const [showLearn, setShowLearn] = useState(false);
  const [cardToEdit, setCardToEdit] = useState(null);

  function handleFormSubmit() {
    setShowForm(false);
    // the submition is handled by the action function above
    setTimeout(() => {
      setCardToEdit(null);
    }, 0);
  }

  function handleCloseForm() {
    setCardToEdit(null);
    setShowForm(false);
  }

  function handleEditCard(cardId) {
    const card = cards.find((x) => x.id === cardId);
    if (!card) {
      // todo: show to error page
      return;
    }
    setCardToEdit(card);
    setShowForm(true);
  }

  async function handleDeleteCard(cardId) {
    await axios.delete(`/v1/decks/${deckId}/cards/${cardId}`);
    revalidator.revalidate();
  }

  return (
    <div className="mt-3">
      <div className="d-flex justify-content-end align-items-center">
        <Button
          variant="outline-primary"
          onClick={() => setShowForm(true)}
        >
          New Card
        </Button>
        <Button className="ms-3" disabled={!cards || !cards.length} onClick={() => setShowLearn(true)}>Learn</Button>
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
                onEdit={handleEditCard}
                onDelete={handleDeleteCard}
              />
            </Col>
          ))}
        </Row>
      ) : (
        <div className="mt-5 text-center">
          <div className="text-secondary">
            Click the &quot;New Card&quot; button to add a card
          </div>
        </div>
      )}

      <Modal show={showForm} onHide={handleCloseForm}>
        <Modal.Header closeButton>
          <Modal.Title>{cardToEdit ? "Edit Card" : "Create Card"}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <RRDForm method="post">
            <Form.Group className="mb-3">
              <Form.Control
                name="word"
                defaultValue={cardToEdit?.word}
                placeholder="Word"
                autoFocus
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Control
                name="meaning"
                defaultValue={cardToEdit?.meaning}
                as="textarea"
                rows={2}
                placeholder="Meaning"
              />
            </Form.Group>
            <Form.Control
              hidden
              name="id"
              defaultValue={cardToEdit ? cardToEdit.id : ""}
            />
            <div className="d-flex">
              <Button
                name="intent"
                defaultValue={cardToEdit ? "edit" : "create"}
                variant="primary"
                type="submit"
                className="ms-auto"
                onClick={handleFormSubmit}
              >
                Save
              </Button>
            </div>
          </RRDForm>
        </Modal.Body>
      </Modal>

      <Modal show={showLearn} fullscreen onHide={() => setShowLearn(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Learn</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Learn cards={cards} />
        </Modal.Body>
      </Modal>
    </div>
  );
}
