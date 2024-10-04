import PropTypes from "prop-types";
import { useState } from "react";
import Button from "react-bootstrap/Button";
import Card from "react-bootstrap/Card";

FlashCard.propTypes = {
  id: PropTypes.number,
  word: PropTypes.string,
  meaning: PropTypes.string,
  example: PropTypes.string,
  onEdit: PropTypes.func,
  onDelete: PropTypes.func,
};

export default function FlashCard({
  id,
  word,
  meaning,
  example,
  onEdit,
  onDelete,
}) {
  const [flipped, setFlipped] = useState(false);

  return (
    <div className={"flip-card" + (flipped ? " flipped" : "")}>
      <div className="flip-card-inner">
        <div className="flip-card-front">
          <Card
            className="bg-secondary text-light"
            style={{ minHeight: 300, minWidth: 220 }}
          >
            <Card.Header>
              {/* <MyTooltip id="edit" content="Edit"> */}
              <Button
                size="sm"
                variant="outline-light"
                className="mx-1"
                onClick={() => onEdit(id)}
              >
                <i className="bi bi-pencil-square"></i>
              </Button>
              {/* </MyTooltip> */}

              {/* <MyTooltip id="delete" content="Delete"> */}
              <Button
                size="sm"
                variant="outline-danger"
                className="mx-1"
                onClick={() => onDelete(id)}
              >
                <i className="bi bi-trash-fill"></i>
              </Button>
              {/* </MyTooltip> */}
            </Card.Header>

            <Card.Body className="d-flex flex-column justify-content-between align-items-center">
              <Card.Title>{word}</Card.Title>
              <Card.Text>{example}</Card.Text>
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
            style={{ minHeight: 300, minWidth: 220 }}
          >
            <Card.Header>
              {/* <MyTooltip id="edit" content="Edit"> */}
              <Button
                size="sm"
                variant="outline-secondary"
                className="mx-1"
                onClick={() => onEdit(id)}
              >
                <i className="bi bi-pencil-square"></i>
              </Button>
              {/* </MyTooltip> */}

              {/* <MyTooltip id="delete" content="Delete"> */}
              <Button
                size="sm"
                variant="outline-danger"
                className="mx-1"
                onClick={() => onDelete(id)}
              >
                <i className="bi bi-trash-fill"></i>
              </Button>
              {/* </MyTooltip> */}
            </Card.Header>

            <Card.Body className="d-flex flex-column justify-content-center">
              <Card.Text>{meaning}</Card.Text>
            </Card.Body>

            <Card.Footer>
              <Button onClick={() => setFlipped(false)}>Hide</Button>
            </Card.Footer>
          </Card>
        </div>
      </div>
    </div>
  );
}
