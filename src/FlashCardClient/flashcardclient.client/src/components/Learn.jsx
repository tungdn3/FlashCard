import PropTypes from "prop-types";
import Carousel from "react-bootstrap/Carousel";
import FlashCard from "./FlashCard";

Learn.propTypes = {
  cards: PropTypes.array,
  onEdit: PropTypes.func,
  onDelete: PropTypes.func,
};

export default function Learn({ cards, onEdit, onDelete }) {
  return (
    <Carousel
      className="bg-secondary bg-opacity-75 w-100 h-100"
      interval={null}
    >
      {cards.map((card) => (
        <Carousel.Item key={card.id} className="">
          <div
            className="d-flex w-100 justify-content-center"
            style={{ marginTop: "10rem", height: 500 }}
          >
            <FlashCard
              id={card.id}
              word={card.word}
              meaning={card.meaning}
              example={card.example}
              onEdit={onEdit}
              onDelete={onDelete}
            />
          </div>
          <Carousel.Caption></Carousel.Caption>
        </Carousel.Item>
      ))}
    </Carousel>
  );
}
