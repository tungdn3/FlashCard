import PropTypes from "prop-types";
import { useState } from "react";
import Button from "react-bootstrap/Button";
import Card from "react-bootstrap/Card";

FlashCard.propTypes = {
  card: PropTypes.object,
  onEdit: PropTypes.func,
  onDelete: PropTypes.func,
};

export default function FlashCard({ card, onEdit, onDelete }) {
  const [flipped, setFlipped] = useState(false);

  return (
    <div className={"flip-card" + (flipped ? " flipped" : "")}>
      <div className="flip-card-inner">
        <div className="flip-card-front">
          <Card
            className="bg-secondary text-light"
            style={{ minHeight: 500, width: 300 }}
          >
            <Card.Header>
              <Button
                size="sm"
                variant="outline-light"
                className="mx-1"
                onClick={() => onEdit(card.id)}
              >
                <i className="bi bi-pencil-square"></i>
              </Button>

              <Button
                size="sm"
                variant="outline-danger"
                className="mx-1"
                onClick={() => onDelete(card.id)}
              >
                <i className="bi bi-trash-fill"></i>
              </Button>
            </Card.Header>

            <Card.Body className="d-flex flex-column justify-content-center align-items-center">
              <Card.Title>{card.word}</Card.Title>
              <Card.Text className="mt-4">{card.example}</Card.Text>
            </Card.Body>

            <Card.Footer>
              <Button variant="outline-light" onClick={() => setFlipped(true)}>
                Show
              </Button>
            </Card.Footer>
          </Card>
        </div>

        <div className="flip-card-back">
          <Card
            className="bg-body-tertiary"
            style={{ minHeight: 500, width: 300 }}
          >
            <Card.Header>
              <Button
                size="sm"
                variant="outline-secondary"
                className="mx-1"
                onClick={() => onEdit(card.id)}
              >
                <i className="bi bi-pencil-square"></i>
              </Button>

              <Button
                size="sm"
                variant="outline-danger"
                className="mx-1"
                onClick={() => onDelete(card.id)}
              >
                <i className="bi bi-trash-fill"></i>
              </Button>
            </Card.Header>

            <Card.Body className="d-flex flex-column justify-content-center">
              <Card.Text>{card.meaning}</Card.Text>
            </Card.Body>

            <Card.Img
              variant="top"
              src={card.imageUrl ?? "/image-placeholder.png"}
              className="mx-auto mb-1"
              style={{ width: 200, height: 200 }}
            />

            <Card.Footer>
              <Button onClick={() => setFlipped(false)}>Hide</Button>
            </Card.Footer>
          </Card>
        </div>
      </div>
    </div>
  );
}
